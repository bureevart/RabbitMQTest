using System.Text.Json.Serialization;
using MassTransit;
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

var rabbitSettings = builder.Configuration.GetSection("RabbitMQ");
var options = rabbitSettings.Get<RabbitSettings>();
builder.Services.Configure<RabbitSettings>(rabbitSettings);
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddOptions<RabbitMqTransportOptions>()
    .Configure(o =>
    {
        o.Host = options.Server;
        o.VHost = options.VirtualHost;
        o.User = options.User;
        o.Pass = options.Password;
    });

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq();
});

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