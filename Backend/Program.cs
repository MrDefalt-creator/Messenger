using Backend.Configuration;
using Backend.Endpoints;
using Backend.Extensions;
using Backend.Interfaces.Auth;
using Backend.Options;
using Backend.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;
        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddDbContext<MessengerContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["ConnectionStrings:DbConnectionString"]);
        });
        
        builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        

        app.AddMappedEndpoints();

        app.Run();
    }
}