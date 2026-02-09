using CamelRegistry.Api.Data;
using CamelRegistry.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidation();
builder.AddCamelsDb();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/api");

app.MapCamelsEndpoints();

app.MigrateDb();

app.Run();
