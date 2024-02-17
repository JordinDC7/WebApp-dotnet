using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockShow.DependencyInjection
{
    public interface IConfigureDependencyInjection
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}