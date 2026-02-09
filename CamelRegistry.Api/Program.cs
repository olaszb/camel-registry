using CamelRegistry.Api.Data;
using CamelRegistry.Api.Endpoints;
using CamelRegistry.Api.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCamelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCamelValidator>();
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
