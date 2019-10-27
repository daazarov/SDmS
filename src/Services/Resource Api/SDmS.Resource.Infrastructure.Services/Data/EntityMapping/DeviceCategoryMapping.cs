using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class DeviceCategoryMapping : IEntityTypeConfiguration<DeviceCategory>
    {
        public void Configure(EntityTypeBuilder<DeviceCategory> builder)
        {
            builder.ToTable("device_categories");

            builder.HasKey(x => x.device_category_id);

            builder.Property(x => x.description).IsRequired();
        }
    }
}
