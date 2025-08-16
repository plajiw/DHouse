using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

using DHouse.Core.Application.Validators.Imoveis;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Infrastructure.Files;
using DHouse.Core.Infrastructure.Raven;
using DHouse.Core.Infrastructure.WhatsApp;

namespace DHouse.Core.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppMvc(this IServiceCollection services)
    {
        services
            .AddControllersWithViews()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Se quiser hot reload de .cshtml em DEV, descomente e instale o pacote:
        // dotnet add src/DHouse.Core.API/DHouse.Core.API.csproj package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation --version 8.*
        // services.AddRazorPages().AddRazorRuntimeCompilation();

        return services; // <<< FALTAVA ESTE RETURN
    }

    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection AddAppValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CriarImovelDtoValidator>();
        return services;
    }

    public static IServiceCollection AddAppAuth(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(o =>
        {
            o.LoginPath = "/admin/auth/login";
            o.AccessDeniedPath = "/admin/auth/forbidden";
            o.SlidingExpiration = true;
            // o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // habilite em PROD/HTTPS
        });

        services.AddAuthorization(o =>
        {
            o.AddPolicy("SomenteAdmin", p => p.RequireRole("Admin"));
        });

        // Hash de senha padrão
        services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
        return services;
    }

    public static IServiceCollection AddRavenDb(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddSingleton<IDocumentStore>(sp =>
        {
            var urls = cfg.GetSection("RavenSettings:Urls").Get<string[]>()
                       ?? throw new InvalidOperationException("RavenSettings:Urls não configurado.");
            var databaseName = cfg.GetValue<string>("RavenSettings:DatabaseName")
                       ?? throw new InvalidOperationException("RavenSettings:DatabaseName não configurado.");

            var store = new DocumentStore { Urls = urls, Database = databaseName };
            store.Initialize();

            // Cria DB se não existir
            try
            {
                store.Maintenance.Server.Send(
                    new CreateDatabaseOperation(new DatabaseRecord(databaseName)));
            }
            catch (ConcurrencyException)
            {
                // Já existe
            }

            // Publica índices do Infrastructure
            IndexCreation.CreateIndexes(typeof(AssemblyReference).Assembly, store);

            // Seed Admin
            SeedAdmin(store, sp);

            return store;
        });

        // Sessão por request
        services.AddScoped(sp => sp.GetRequiredService<IDocumentStore>().OpenAsyncSession());
        return services;
    }

    public static IServiceCollection AddImageStorage(this IServiceCollection services)
    {
        services.AddSingleton<IImageStorage, LocalImageStorage>();
        return services;
    }

    public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(o =>
        {
            o.AddFixedWindowLimiter("Public", options =>
            {
                options.PermitLimit = 30;         // 30 req/min
                options.Window = TimeSpan.FromMinutes(1);
                options.QueueLimit = 0;
            });
        });
        return services;
    }

    public static IServiceCollection AddWhatsApp(this IServiceCollection services, IConfiguration cfg)
    {
        services.Configure<WhatsAppOptions>(cfg.GetSection("WhatsApp"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<WhatsAppOptions>>().Value);
        services.AddHttpClient<IWhatsAppClient, WhatsAppCloudClient>();
        return services;
    }

    // --------- helpers ---------
    private static void SeedAdmin(IDocumentStore store, IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();

        using var session = store.OpenSession();
        var exists = session.Query<Usuario>().Any(u => u.Roles.Contains("Admin"));
        if (exists) return;

        var senhaHash = hasher.HashPassword(null!, "Admin@123"); // TROCAR depois
        var admin = Usuario.Create("admin@dhouse.local", "Administrador", senhaHash, new[] { "Admin" });
        session.Store(admin);
        session.SaveChanges();
    }
}
