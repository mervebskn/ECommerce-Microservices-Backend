using AuthService.Abstractions;
using AuthService.Configurations;
using AuthService.Repositories;
using Common.Utils;
using Consul;
using Microsoft.AspNetCore.Authentication;
using ServiceDiscoveryConsul;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Consul client yapýlandýrmasý
builder.Services.AddSingleton<IConsulClient, ConsulClient>(sp =>
{
    return new ConsulClient(config =>
    {
        config.Address = new Uri("http://localhost:8500"); //consul url
    });
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtUtils>();
builder.Services.AddScoped<IAuthService, AuthService.Services.AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


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

app.UseAuthorization();

var serviceId = $"{builder.Environment.ApplicationName}-{Guid.NewGuid()}";  // Benzersiz servis ID'si
var serviceName = "auth-service";  // Servis adý
var servicePort = 5212;
var serviceAddress = "auth_service"; //container-name

var consulService = new ConsulServiceRegistration(serviceId, serviceName, servicePort, serviceAddress);
await consulService.RegisterService();

//uygulama kapanýrken Consul kaydýný siler
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(async () =>
{
    await consulService.DeregisterService();
});

app.MapControllers();

app.Run();
