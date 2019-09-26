using SDmS.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SDmS.Identity.Core.EntityMapping
{
    public class AppExceptionMapping : EntityTypeConfiguration<AppException>
    {
        public AppExceptionMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Action).IsRequired();
            Property(x => x.Controller).IsRequired();
            Property(x => x.StackTrace).IsRequired();
            Property(x => x.Message).IsRequired();
        }
    }
}
