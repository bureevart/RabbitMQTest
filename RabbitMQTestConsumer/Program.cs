using System.Text.Json.Serialization;
using RabbitMQTestConsumer.HostedServices;
using RabbitMQTestConsumer.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("rabbit.json");
builder.Services.Configure<RabbitSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddHostedService<ConsumerService>();


builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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