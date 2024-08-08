using System.Text.Json.Serialization;
using RabbitMQTestPublisher.Options;
using RabbitMQTestPublisher.Services;
using RabbitMQTestPublisher.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("rabbit.json");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RabbitSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped<IMessageService, MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();