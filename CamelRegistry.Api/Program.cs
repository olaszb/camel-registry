using CamelRegistry.Api.Data;
using CamelRegistry.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddCamelsDb();

var app = builder.Build();

app.UsePathBase("/api");

app.MapCamelsEndpoints();

app.MigrateDb();

app.Run();
