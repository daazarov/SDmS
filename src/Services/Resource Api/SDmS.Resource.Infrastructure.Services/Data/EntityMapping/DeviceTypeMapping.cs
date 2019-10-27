using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class DeviceTypeMapping : IEntityTypeConfiguration<DeviceType>
    {
        public void Configure(EntityTypeBuilder<DeviceType> builder)
        {
            builder.ToTable("device_types");

            builder.HasKey(x => x.device_type_id);

            builder.Property(x => x.description).IsRequired();

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Types)
                .HasForeignKey(x => x.device_category_id);
        }
    }
}
