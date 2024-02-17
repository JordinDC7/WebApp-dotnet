using RockShow.Domain.AppSettings;
using RockShow.Security;
using RockShow.Security.Configs;
using RockShow.StartUp;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        DependencyInjection.ConfigureServices(builder.Services, builder.Configuration);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();


        var app = builder.Build();

      
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); // Use developer exception page in development
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseDefaultFiles(); // Use default files such as index.html in the wwwroot folder
        app.UseStaticFiles(); // Serve static files from wwwroot
                              
        app.UseCors(policy => policy // Apply the CORS policy
        .WithOrigins("https://oracleillusions.azurewebsites.net", "http://localhost:3000", "https://localhost:7286", "http://127.0.0.1:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());


        app.UseRouting(); // Configure routing



        app.UseAuthentication(); // Note: Authentication should typically come before Authorization
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html"); 
        });

        


        app.Run(); // Run the application
    }
}
