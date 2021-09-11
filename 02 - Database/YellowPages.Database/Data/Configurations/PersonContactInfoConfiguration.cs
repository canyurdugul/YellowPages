using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Entities.Person;

namespace YellowPages.Database.Data.Configurations
{
    public class PersonContactInfoConfiguration : IEntityTypeConfiguration<PersonContactInfo>
    {
        public void Configure(EntityTypeBuilder<PersonContactInfo> builder)
        {
            builder.ToTable("PersonContactInfo");
            builder.HasQueryFilter(f => f.IsDeleted == false);
            builder.Property(s => s.ContactType);
            builder.Property(s => s.PersonId);
            builder.Property(s => s.ContactInfo).HasMaxLength(100);
            builder.Property(s => s.IsDeleted).HasDefaultValue(false);

            builder.HasOne(x => x.Person);
                
        }
    }
}
