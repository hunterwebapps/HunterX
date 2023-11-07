using HunterX.Trader.Infrastructure.Startup;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .AddConfiguration()
    .AddLogging()
    .AddMassTransit()
    .AddProfiles()
    .RegisterDependencies();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
