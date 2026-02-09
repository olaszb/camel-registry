using System;
using CamelRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Api.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CamelRegistryContext>();
        await context.Database.MigrateAsync();
    }

    public static void AddCamelsDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddSqlite<CamelRegistryContext>(connectionString,
        optionsAction: options =>
        {
            options.UseSeeding((context, _) =>
            {
                SeedData(context);
            });

            options.UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                await SeedDataAsync(context);
            });
        });
    }

private static void SeedData(DbContext context)
    {
        if (!context.Set<Camel>().Any())
        {
            context.Set<Camel>().AddRange(GetInitialCamels());
            context.SaveChanges();
        }
    }

    private static async Task SeedDataAsync(DbContext context)
    {
        if (!await context.Set<Camel>().AnyAsync())
        {
            await context.Set<Camel>().AddRangeAsync(GetInitialCamels());
            await context.SaveChangesAsync();
        }
    }

    private static List<Camel> GetInitialCamels() => [
        new() { Name = "Alice", Color = "Brown", HumpCount = 1, LastFed = DateTime.UtcNow.AddDays(-1) },
        new() { Name = "Bob", Color = "White", HumpCount = 2, LastFed = DateTime.UtcNow.AddDays(-2) }
    ];
}
