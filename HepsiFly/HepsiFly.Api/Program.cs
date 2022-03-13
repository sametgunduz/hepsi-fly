using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using HepsiFly.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation()
    .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataConfiguration(builder.Configuration);
builder.Services.AddMediatRServices();
builder.Services.AddValidations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseGlobalExceptionMiddleware()
    .UseRequestAndResponseLoggingMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();