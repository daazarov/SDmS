using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class ActionParameterBindingMapper : IEntityTypeConfiguration<ActionParameterBinding>
    {
        public void Configure(EntityTypeBuilder<ActionParameterBinding> builder)
        {
            builder.ToTable("action_parameter_bindings");

            builder.HasKey(x => new { x.action_id, x.action_parameter_id});

            builder.HasOne(x => x.Action)
                .WithMany(x => x.Bindings)
                .HasForeignKey(x => x.action_id);

            builder.HasOne(x => x.Parameter)
                .WithMany(x => x.Bindings)
                .HasForeignKey(x => x.action_parameter_id);
        }
    }
}
