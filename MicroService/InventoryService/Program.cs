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

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>();
builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

// Event Handler'ý sisteme tanýtma
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

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<ProductCreatedEvent, ProductCreatedEventHandler>();
eventBus.Subscribe<OrderCreatedEvent, OrderCreatedEventHandler>();

app.Run();
