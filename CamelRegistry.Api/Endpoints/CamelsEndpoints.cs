using System;
using CamelRegistry.Api.Data;
using CamelRegistry.Api.Dtos;
using CamelRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Api.Endpoints;

public static class CamelsEndpoints
{   
    const string GetCamelEndpointName = "GetCamel";

    public static void MapCamelsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/camels");

        //GET /api/camels
        group.MapGet("/", async (CamelRegistryContext dbContext)
             => await dbContext.Camels
             .Select(camel => new CamelDto(
                camel.Id,
                camel.Name,
                camel.Color,
                camel.HumpCount,
                camel.LastFed
        )).ToListAsync());
        
        // GET /api/camels/{id}
        group.MapGet("/{id}", async (int id, CamelRegistryContext dbContext) =>
        {
            var camel = await dbContext.Camels.FindAsync(id);

            return camel is null ? Results.NotFound() 
                : Results.Ok(new CamelDto(
                    camel.Id,
                    camel.Name,
                    camel.Color,
                    camel.HumpCount,
                    camel.LastFed
                ));
        }).WithName(GetCamelEndpointName);
        
        // POST /api/camels
        group.MapPost("/", async (CreateCamelDto newCamelDto, CamelRegistryContext dbContext) =>
        {
            Camel camel = new()
            {
                Name = newCamelDto.Name,
                Color = newCamelDto.Color,
                HumpCount = newCamelDto.HumpCount,
                LastFed = newCamelDto.LastFed
            };
            dbContext.Camels.Add(camel);
            await dbContext.SaveChangesAsync();

            CamelDto camelDto = new(
                camel.Id,
                camel.Name,
                camel.Color,
                camel.HumpCount,
                camel.LastFed
            );

            return Results.CreatedAtRoute(GetCamelEndpointName, new { id = camel.Id }, camelDto);
        });
        
        // PUT /api/camels/{id}
        group.MapPut("/{id}", async (int id, UpdateCamelDto updateCamelDto, CamelRegistryContext dbContext) =>
        {
            var camel = await dbContext.Camels.FindAsync(id);

            if (camel is null)
            {
                return Results.NotFound();
            }

            camel.Name = updateCamelDto.Name;
            camel.Color = updateCamelDto.Color;
            camel.HumpCount = updateCamelDto.HumpCount;
            camel.LastFed = updateCamelDto.LastFed;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
        
        group.MapDelete("/{id}", async (int id, CamelRegistryContext dbContext) =>
        {
            await dbContext.Camels.Where(c => c.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
