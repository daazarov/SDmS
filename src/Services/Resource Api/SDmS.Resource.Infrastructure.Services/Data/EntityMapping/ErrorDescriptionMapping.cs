using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDmS.Resource.Common.Entities;

namespace SDmS.Resource.Infrastructure.Services.Data.EntityMapping
{
    public class ErrorDescriptionMapping : IEntityTypeConfiguration<ErrorDescription>
    {
        public void Configure(EntityTypeBuilder<ErrorDescription> builder)
        {
            builder.ToTable("error_descriptions");

            builder.HasKey(x => x.error_id);

            builder.Property(x => x.error_code).IsRequired();
            builder.Property(x => x.description).IsRequired();
        }
    }
}
