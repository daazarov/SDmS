using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class DeviceParameterMapping : IEntityTypeConfiguration<DeviceParameter>
    {
        public void Configure(EntityTypeBuilder<DeviceParameter> builder)
        {
            builder.ToTable("device_parameters");

            builder.HasKey(x => x.parameter_id);

            builder.Property(x => x.description).IsRequired();
        }
    }
}
