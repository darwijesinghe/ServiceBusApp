using Microsoft.Extensions.Azure;
using SharedLibrary.Configuration;


var builder = WebApplication.CreateBuilder(args);
var config  = builder.Configuration;

// Add services to the container.

// Bind configuration
builder.Services.Configure<ServiceBusOptions>(builder.Configuration.GetSection("AzureServiceBus"));

// Register Service Bus Client from the shared library
builder.Services.AddServiceBusClient(builder.Configuration);


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
