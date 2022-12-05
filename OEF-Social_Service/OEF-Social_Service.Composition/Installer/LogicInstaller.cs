using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OEF_Social_Service.Composition.Installer.Interfaces;
using OEF_Social_Service.DataAccess.Data.Services.Interfaces;
using OEF_Social_Service.Services;
using OEF_Social_Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.Composition.Installer
{
    public  class LogicInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserLogic, UserLogic>();
        }
    }
}
