using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDmS.Resource.Common.Entities.Devices;
using SDmS.Resource.Domain.Models.Devices;

namespace SDmS.Resource.DI.Modules
{
    public class ModelMapperModule : IModule
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                               AppDomain.CurrentDomain.GetAssemblies());
        }
    }

    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DeviceParameterValue, DeviceParameterValueDomain>()
                .ForMember("parameter_name", opt => opt.MapFrom(c => c.Parameter.description));
            CreateMap<DeviceParameterValueDomain, DeviceParameterValue>();

            CreateMap<Device, DeviceDomainModel>();
            CreateMap<DeviceDomainModel, Device>();
        }
    }
}
