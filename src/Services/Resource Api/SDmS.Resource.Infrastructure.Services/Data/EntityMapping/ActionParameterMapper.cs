using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities.Devices;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class ActionParameterMapper : IEntityTypeConfiguration<ActionParameter>
    {
        public void Configure(EntityTypeBuilder<ActionParameter> builder)
        {
            builder.ToTable("action_parameters");

            builder.HasKey(x => x.action_parameter_id);

            builder.Property(x => x.description).IsRequired(); ;
        }
    }
}
