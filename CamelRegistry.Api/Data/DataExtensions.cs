using System;
using CamelRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CamelRegistryContext>();
        context.Database.Migrate();
    }

    public static void AddCamelsDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddSqlite<CamelRegistryContext>(
            connectionString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Camel>().Any())
                {
                    context.Set<Camel>().AddRange(
                        new Camel
                        {
                            Name = "Alice",
                            Color = "Brown",
                            HumpCount = 1,
                            LastFed = DateTime.UtcNow.AddDays(-1)
                        },
                        new Camel
                        {
                            Name = "Bob",
                            Color = "White",
                            HumpCount = 2,
                            LastFed = DateTime.UtcNow.AddDays(-2)
                        }
                    );
                    context.SaveChanges();
                }
            }));
    }
}
