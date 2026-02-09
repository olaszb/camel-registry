using System.Net;
using System.Net.Http.Json;
using CamelRegistry.Api.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CamelRegistry.Tests;

public class CamelApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CamelApiTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCamels_ReturnsSuccessAndSeedData()
    {
        var response = await _client.GetAsync("/api/camels");

        response.EnsureSuccessStatusCode();
        var camels = await response.Content.ReadFromJsonAsync<List<CamelDto>>();
        Assert.NotNull(camels);
        Assert.NotEmpty(camels);
    }

    [Fact]
    public async Task GetCamel_ReturnsSuccessAndCamel()
    {
        
        var camels = await (await _client.GetAsync("/api/camels")).Content.ReadFromJsonAsync<List<CamelDto>>();
        
        Assert.NotNull(camels);
        Assert.NotEmpty(camels);
        var targetId = camels.First().Id;

        var response = await _client.GetAsync($"/api/camels/{targetId}");
    
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var camel = await response.Content.ReadFromJsonAsync<CamelDto>();
        Assert.NotNull(camel);
        Assert.Equal(targetId, camel.Id);
    }

    [Fact]
    public async Task GetCamel_ReturnsNotFound_ForNonExistentId()
    {
        var response = await _client.GetAsync("/api/camels/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateCamel_ReturnsBadRequest_ForInvalidData()
    {
        var newCamel = new CreateCamelDto("", "Brown", 3, DateTime.UtcNow);
        var response = await _client.PostAsJsonAsync("/api/camels", newCamel);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCamel_ReturnsCreated_ForValidData()
    {
        var newCamel = new CreateCamelDto("Test Camel", "Brown", 1, DateTime.UtcNow.AddDays(-1));
        var response = await _client.PostAsJsonAsync("/api/camels", newCamel);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdCamel = await response.Content.ReadFromJsonAsync<CamelDto>();
        Assert.NotNull(createdCamel);
        Assert.Equal(newCamel.Name, createdCamel.Name);
    }
    [Fact]
    public async Task UpdateCamel_ReturnsNotFound_ForNonExistentId()
    {
        var updatedCamel = new UpdateCamelDto("Updated Camel", "Blue", 2, DateTime.UtcNow.AddDays(-1));
        var response = await _client.PutAsJsonAsync("/api/camels/9999", updatedCamel);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task UpdateCamel_ReturnsBadRequest_ForInvalidData()
    {
        var updatedCamel = new UpdateCamelDto("", "Blue", 2, DateTime.UtcNow.AddDays(-1));

        var camels = await (await _client.GetAsync("/api/camels")).Content.ReadFromJsonAsync<List<CamelDto>>();
        
        Assert.NotNull(camels);
        Assert.NotEmpty(camels);
        var targetId = camels.First().Id;

        var response = await _client.PutAsJsonAsync($"/api/camels/{targetId}", updatedCamel);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCamel_ReturnsSuccess_ForValidData()
    {
        var updatedCamel = new UpdateCamelDto("Updated Camel", "Blue", 2, DateTime.UtcNow.AddDays(-1));

        var camels = await (await _client.GetAsync("/api/camels")).Content.ReadFromJsonAsync<List<CamelDto>>();
        
        Assert.NotNull(camels);
        Assert.NotEmpty(camels);
        var targetId = camels.First().Id;

        var response = await _client.PutAsJsonAsync($"/api/camels/{targetId}", updatedCamel);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCamel_ReturnsNotFound_ForNonExistentId()
    {
        var response = await _client.DeleteAsync("/api/camels/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCamel_ReturnsNoContent_ForExistingId()
    {
        var camels = await (await _client.GetAsync("/api/camels")).Content.ReadFromJsonAsync<List<CamelDto>>();
        
        Assert.NotNull(camels);
        Assert.NotEmpty(camels);
        var targetId = camels.First().Id;

        var response = await _client.DeleteAsync($"/api/camels/{targetId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}