using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class DeviceMapping : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("devices");

            builder.HasKey(x => x.device_id);

            builder.Property(x => x.creation_date).HasDefaultValueSql("getdate()");
            builder.Property(x => x.device_type_id).IsRequired();
            builder.Property(x => x.is_online).IsRequired();
            builder.Property(x => x.name).IsRequired();
            builder.Property(x => x.serial_number).IsRequired();
            builder.Property(x => x.user_id).IsRequired();

            builder.HasOne(x => x.Type).WithMany(x => x.Devices).HasForeignKey(x => x.device_type_id);
        }
    }
}
