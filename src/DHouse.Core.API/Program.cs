using System.Text.Json.Serialization;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;

using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;          // GetStatisticsOperation
using Raven.Client.Documents.Linq;                // Query/Any
using Raven.Client.Exceptions;                    // ConcurrencyException
using Raven.Client.ServerWide;                    // DatabaseRecord
using Raven.Client.ServerWide.Operations;         // CreateDatabaseOperation

using DHouse.Core.Application.Validators.Imoveis; // âncora p/ validators
using DHouse.Core.Infrastructure.Raven;           // AssemblyReference (índices)
using DHouse.Core.Infrastructure.Files;           // IImageStorage/LocalImageStorage
using DHouse.Core.Domain.Entities;                // Usuario

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews() // habilita MVC + Views + TempData
    .AddJsonOptions(o => { o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation (validação automática de DTOs)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CriarImovelDtoValidator>();

// Auth (Cookie para Admin)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(o =>
{
    o.LoginPath = "/admin/auth/login";
    o.AccessDeniedPath = "/admin/auth/forbidden";
    o.SlidingExpiration = true;
    // o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // habilite em produção/HTTPS
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("SomenteAdmin", p => p.RequireRole("Admin"));
});

// Hash de senha padrão
builder.Services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

// RavenDB: DocumentStore (Singleton) + criação de DB + índices + seed Admin
builder.Services.AddSingleton<IDocumentStore>(sp =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var urls = cfg.GetSection("RavenSettings:Urls").Get<string[]>()
               ?? throw new InvalidOperationException("RavenSettings:Urls não configurado.");
    var databaseName = cfg.GetValue<string>("RavenSettings:DatabaseName")
               ?? throw new InvalidOperationException("RavenSettings:DatabaseName não configurado.");

    var store = new DocumentStore { Urls = urls, Database = databaseName };
    store.Initialize();

    // Cria a database se ainda não existir (idempotente)
    try
    {
        store.Maintenance.Server.Send(
            new CreateDatabaseOperation(new DatabaseRecord(databaseName))
        );
    }
    catch (ConcurrencyException)
    {
        // Já existe — seguir
    }

    // Publica índices estáticos do assembly de Infrastructure
    IndexCreation.CreateIndexes(typeof(AssemblyReference).Assembly, store);

    // Seed do usuário Admin (executa uma vez)
    SeedAdmin(store, sp);

    return store;
});

// Sessão Raven por request (Unit of Work)
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IDocumentStore>().OpenAsyncSession());

// Armazenamento de imagens (local)
builder.Services.AddSingleton<IImageStorage, LocalImageStorage>();

var app = builder.Build();

// Servir arquivos estáticos (wwwroot) + cache para imagens
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // 30 dias de cache + immutable
        ctx.Context.Response.Headers.CacheControl = "public,max-age=2592000,immutable";
    }
});

// Swagger somente em Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Redireciona para HTTPS em ambientes não-dev
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

// Rota da Área Admin (MVC)
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}"
);

// API (controllers)
app.MapControllers();

// Healthcheck Raven
app.MapGet("/health/raven", (IDocumentStore store) =>
{
    var stats = store.Maintenance.Send(new GetStatisticsOperation());
    return Results.Ok(new
    {
        stats.DatabaseId,
        stats.DatabaseChangeVector,
        stats.CountOfDocuments
    });
})
.WithName("RavenHealth")
.Produces(StatusCodes.Status200OK);

app.Run();

static void SeedAdmin(IDocumentStore store, IServiceProvider sp)
{
    using var scope = sp.CreateScope();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();

    using var session = store.OpenSession();
    // existe algum Admin?
    var exists = session.Query<Usuario>().Any(u => u.Roles.Contains("Admin"));
    if (exists) return;

    var senhaHash = hasher.HashPassword(null!, "Admin@123"); // TROCAR depois!
    var admin = Usuario.Create("admin@dhouse.local", "Administrador", senhaHash, new[] { "Admin" });
    session.Store(admin);
    session.SaveChanges();
}
