using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.StartUp.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public class AConfigureServices : IConfigureDependencyInjection
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            
        }
    }
}