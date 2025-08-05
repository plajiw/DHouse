using DHouse.Core.Infrastructure; // Verifique se este using está aqui
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDocumentStore>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    var urls = configuration.GetSection("RavenSettings:Urls").Get<string[]>();
    var databaseName = configuration.GetValue<string>("RavenSettings:DatabaseName");

    var store = new DocumentStore
    {
        Urls = urls,
        Database = databaseName
    };

    store.Initialize();

    IndexCreation.CreateIndexes(typeof(AssemblyReference).Assembly, store);

    return store;
});

builder.Services.AddScoped(provider => provider.GetRequiredService<IDocumentStore>().OpenAsyncSession());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();