using Microsoft.Extensions.DependencyInjection;
using SDmS.DeviceListener.Core.Interfaces.Services;
using SDmS.DeviceListener.Core.Services;
using SDmS.DeviceListener.Host.Extensions;
using SDmS.DeviceListener.Infrastructure.Data.Contexts;
using SDmS.DeviceListener.Infrastructure.Data.Repositories;
using SDmS.DeviceListener.Infrastructure.Interfaces.Data;
using SDmS.DeviceListener.Infrastructure.Interfaces.Repositories;

namespace SDmS.DeviceListener.Host
{
    public class InfrastructureModule : IModule
    {
        public void Register(IServiceCollection services)
        {
            services.AddTransient<IDeviceListenerContext, DeviceListenerContext>();
            services.AddTransient<IClimateRepository, ClimateRepository>();
            services.AddTransient<ITempSensorRepository, TempSensorRepository>();
            services.AddTransient<ILedRepository, LedRepository>();
        }
    }

    public class CoreModule : IModule
    {
        public void Register(IServiceCollection services)
        {
            //services.AddTransient<IClimateService, ClimateService>();
            //services.AddTransient<ILedService, LedService>();
            //services.AddTransient<ITempSensorService, TempSensorService>();
            services.AddTransient<INewDeviceService, NewDeviceService>();
        }
    }
}
