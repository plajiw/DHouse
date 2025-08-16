using DHouse.Core.API.Extensions; // << nossas extensions

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services
    .AddAppMvc()
    .AddAppSwagger()
    .AddAppValidation()
    .AddAppAuth()
    .AddRavenDb(builder.Configuration)
    .AddImageStorage()
    .AddAppRateLimiting(); // opcional: limita /api pública e hooks

// (Opcional, quando você colar os arquivos do WhatsApp)
 builder.Services.AddWhatsApp(builder.Configuration);

var app = builder.Build();

// Pipeline
app.UseAppStaticFiles();
app.UseAppSwagger();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter(); // se AddAppRateLimiting foi chamado

app.MapAdminArea();
app.MapControllers();
app.MapHealth();

app.Run();
