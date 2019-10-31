using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class ActionMapping : IEntityTypeConfiguration<Action>
    {
		public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.ToTable("device_actions");

            builder.HasKey(x => x.action_id);

            builder.Property(x => x.description).IsRequired();

            builder.HasOne(x => x.DeviceType)
				.WithMany(g => g.Actions)
				.HasForeignKey(x => x.device_type_id);
        }
    }
}
