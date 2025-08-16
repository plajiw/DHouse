using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using Raven.Client.Documents;
using Raven.Client.Documents.Operations;

namespace DHouse.Core.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseAppStaticFiles(this WebApplication app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.CacheControl = "public,max-age=2592000,immutable"; // 30 dias
            }
        });
        return app;
    }

    public static WebApplication UseAppSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }
        return app;
    }

    public static WebApplication MapAdminArea(this WebApplication app)
    {
        app.MapAreaControllerRoute(
            name: "admin",
            areaName: "Admin",
            pattern: "admin/{controller=Home}/{action=Index}/{id?}"
        );
        return app;
    }

    public static WebApplication MapHealth(this WebApplication app)
    {
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

        return app;
    }
}
