using System;
using CamelRegistry.Api.Data;
using CamelRegistry.Api.Dtos;
using CamelRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Api.Endpoints;

public static class CamelsEndpoints
{   
    const string GetCamelEndpointName = "GetCamel";
    const string GetCamelsEndpointName = "GetCamels";
    const string CreateCamelEndpointName = "CreateCamel";
    const string UpdateCamelEndpointName = "UpdateCamel";
    const string DeleteCamelEndpointName = "DeleteCamel";

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
        )).ToListAsync()).WithName(GetCamelsEndpointName)
            .Produces<List<CamelDto>>(StatusCodes.Status200OK)
            .WithSummary("Get all camels")
            .WithDescription("Returns a list of all camels in the registry.");
        
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
        }).WithName(GetCamelEndpointName)
            .Produces<CamelDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get a camel by id")
            .WithDescription("Returns a single camel with the specified id. If no camel is found, returns a 404 Not Found response.");
        
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
        }).WithName(CreateCamelEndpointName)
            .Produces<CamelDto>(StatusCodes.Status201Created)
            .WithSummary("Create a new camel")
            .WithDescription("Creates a new camel in the registry with the provided information. Returns the created camel with its assigned id.");
        
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
        }).WithName(UpdateCamelEndpointName)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Update an existing camel")
            .WithDescription("Updates the information of an existing camel with the specified id. If no camel is found, returns a 404 Not Found response. If the update is successful, returns a 204 No Content response.");
        
        group.MapDelete("/{id}", async (int id, CamelRegistryContext dbContext) =>
        {
            // await dbContext.Camels.Where(c => c.Id == id).ExecuteDeleteAsync();

            var camel = await dbContext.Camels.FindAsync(id);

            if (camel is null)
            {
                return Results.NotFound();
            }

            dbContext.Camels.Remove(camel);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();

        }).WithName(DeleteCamelEndpointName)
            .Produces(StatusCodes.Status204NoContent)
            .WithSummary("Delete a camel")
            .WithDescription("Deletes the camel with the specified id from the registry. If the deletion is successful, returns a 204 No Content response.");
    }
}
