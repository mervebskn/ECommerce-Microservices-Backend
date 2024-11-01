using AuthService.Abstractions;
using AuthService.Configurations;
using AuthService.Repositories;
using Common.Utils;
using Consul;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

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

var consulClient = app.Services.GetRequiredService<IConsulClient>();
var serviceId = $"{builder.Environment.ApplicationName}-{Guid.NewGuid()}"; // Servis ID'si
var registration = new AgentServiceRegistration()
{
    ID = serviceId,
    Service = new AgentService
    {
        ID = serviceId,
        Service = builder.Environment.ApplicationName,
        Address = "localhost", // Servis adresi
        Port = 5000, // Servis portu
        Tags = new[] { "auth", "api" } // Gerekirse etiketler ekleyin
    }
};

await consulClient.Agent.ServiceRegister(registration);

app.MapControllers();

app.Run();
