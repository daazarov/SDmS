using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class DeviceParameterBindingMapper : IEntityTypeConfiguration<DeviceParameterBinding>
    {
        public void Configure(EntityTypeBuilder<DeviceParameterBinding> builder)
        {
            builder.ToTable("device_parameter_bindings");

            builder.HasKey(x => new { x.device_type_id, x.parameter_id });

            builder
               .HasOne(x => x.Parameter)
               .WithMany(x => x.Bindings)
               .HasForeignKey(x => x.parameter_id);

            builder
                .HasOne(x => x.Type)
                .WithMany(x => x.Bindings)
                .HasForeignKey(x => x.device_type_id);
        }
    }
}
