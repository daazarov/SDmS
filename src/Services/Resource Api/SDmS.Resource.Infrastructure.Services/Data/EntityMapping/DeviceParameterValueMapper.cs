using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class DeviceParameterValueMapper : IEntityTypeConfiguration<DeviceParameterValue>
    {
        public void Configure(EntityTypeBuilder<DeviceParameterValue> builder)
        {
            builder.ToTable("device_parameter_values");

            builder.HasKey(x => new { x.device_id, x.parameter_id});

            builder
                .HasOne(x => x.Parameter)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.parameter_id);

            builder
                .HasOne(x => x.Device)
                .WithMany(x => x.DeviceParameters)
                .HasForeignKey(x => x.device_id);
        }
    }
}
