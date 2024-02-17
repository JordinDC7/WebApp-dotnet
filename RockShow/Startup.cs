using RockShow.Domain.AppSettings;
using RockShow.Security;
using RockShow.Security.Configs;
using RockShow.StartUp;

using System;
using SecurityConfig = RockShow.Security.Configs.SecurityConfig;

namespace RockShow.StartUp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMemoryCache();

            ConfigureAppSettings(services);
            DependencyInjection.ConfigureServices(services, Configuration);
            Authentication.ConfigureServices(services, Configuration);



        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<RockShow.Security.Configs.SecurityConfig>(Configuration.GetSection("SecurityConfig"));
            services.Configure<JsonWebTokenConfig>(Configuration.GetSection("JsonWebTokenConfig"));
            services.Configure<AppKeys>(Configuration.GetSection("AppKeys"));


        }
    }
}
