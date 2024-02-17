using RockShow.DependencyInjection;


namespace RockShow.StartUp
{
    public class AConfigureService
    {
        public class AConfigureServices : IConfigureDependencyInjection
        {
            public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
            {

            }
        }
    }
}
