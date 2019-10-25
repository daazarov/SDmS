using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using NServiceBus.ObjectBuilder.Common;

namespace SDmS.DeviceListener.Host
{
    public class CustomIoCServiceProviderFactory : IServiceProviderFactory<IContainer>
    {
        public IContainer CreateBuilder(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public IServiceProvider CreateServiceProvider(IContainer containerBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
