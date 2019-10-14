using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.DeviceEnactor.Host.Extensions
{
    public static class AddMqttMessageHandlersExtensions
    {
        public static IServiceCollection AddMqttMessageHandlers(this IServiceCollection services)
        {
            return services;
        }
    }
}
