using Consul;
using EventBus.Abstractions;
using EventBus.Connections;
using EventBus.Events;
using EventBus.Implementations;
using InventoryService.Abstractions;
using InventoryService.Data;
using InventoryService.EventHandlers;
using InventoryService.Repositories;
using InventoryService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using ServiceDiscoveryConsul;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var connectionFactory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBus:HostName"],
        DispatchConsumersAsync = true,
        //UserName = builder.Configuration["EventBus:UserName"],
        //Password = builder.Configuration["EventBus:Password"]
    };
    return new RabbitMQPersistentConnection(connectionFactory);
});

//consul client yap�land�r
builder.Services.AddSingleton<IConsulClient, ConsulClient>(sp =>
{
    return new ConsulClient(config =>
    {
        config.Address = new Uri("http://localhost:8500"); //consul url
    });
});

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>();
builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

// Event Handler'� sisteme tan�tma
builder.Services.AddTransient<IIntegrationEventHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
builder.Services.AddTransient<IIntegrationEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InvtService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

//consul servis kayd�
var consulServiceId = $"{builder.Environment.ApplicationName}-{Guid.NewGuid()}";
var consulService = new ConsulServiceRegistration(
    consulServiceId,
    "inventory-service", //servis ad�
    5246, // Servis portu
    "inventory_service" //docker konteyner ad�
);
await consulService.RegisterService();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(async () =>
{
    await consulService.DeregisterService();
});

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<ProductCreatedEvent, ProductCreatedEventHandler>();
eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

app.Run();
