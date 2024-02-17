using Microsoft.AspNetCore.Connections;
using RockShow.Data;
using RockShow.Domain.AppSettings;
using RockShow.Interfaces;
using RockShow.Security;
using RockShow.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using RockShow.StartUp;
using Microsoft.AspNetCore.Authentication;
using Sabio.Data.Providers;
using RockShow.DependencyInjection;

namespace RockShow.StartUp
{
 
        public class DependencyInjection
        {
            public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
            {
                // Register IConfiguration
                services.AddSingleton<IConfiguration>(configuration);

                // Retrieve connection string from configuration
                string connectionString = configuration.GetConnectionString("DefaultConnection");

                // Register other services
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddSingleton<IIdentityProvider<int>, WebAuthenticationService>();
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<ILookUpService, LookUpService>();
                services.AddSingleton<IRockServiceNew, RockServiceNew>();
                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<IDatabaseProcCommands, DatabaseProcCommands>();
                Authentication.ConfigureServices(services, configuration);
                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<IAuthenticationService<int>, WebAuthenticationService>();


        


            // Register AppKeys configuration
            services.Configure<AppKeys>(configuration.GetSection("AppKeys"));

                // Register SwaggerGen
                services.AddSwaggerGen();

                // Add MVC services
                services.AddControllersWithViews();

                // Add HttpClient
                services.AddHttpClient();
            }
        }
    }



