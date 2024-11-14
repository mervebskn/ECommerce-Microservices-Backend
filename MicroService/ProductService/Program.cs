using Consul;
using EventBus.Abstractions;
using EventBus.Connections;
using EventBus.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductService.Abstractions;
using ProductService.Data;
using ProductService.Repositories;
using ProductService.Services;
using RabbitMQ.Client;
using ServiceDiscoveryConsul;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Database baðlantýsý
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ baðlantýsý ve Event Bus konfigürasyonu
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var connectionFactory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBus:HostName"],
        UserName = builder.Configuration["EventBus:UserName"],
        Password = builder.Configuration["EventBus:Password"],
        DispatchConsumersAsync = true
    };
    return new RabbitMQPersistentConnection(connectionFactory);
});

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>();
builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});

//consul client yapýlandýr
builder.Services.AddSingleton<IConsulClient, ConsulClient>(sp =>
{
    return new ConsulClient(config =>
    {
        config.Address = new Uri("http://localhost:8500"); //consul url
    });
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProdService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//consul servis kaydý
var consulServiceId = $"{builder.Environment.ApplicationName}-{Guid.NewGuid()}";
var consulService = new ConsulServiceRegistration(
    consulServiceId,
    "product-service", //servis adý
    5017, //servis portu
    "product_service" //docker konteyner adý
);
await consulService.RegisterService();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(async () =>
{
    await consulService.DeregisterService();
});

app.MapControllers();

app.Run();
