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
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");
            builder.HasQueryFilter(f => f.IsDeleted == false);
            builder.Property(s => s.Name).HasMaxLength(100);
            builder.Property(s => s.Surname).HasMaxLength(500);
            builder.Property(s => s.FirmName).HasMaxLength(200);
            builder.Property(s => s.IsDeleted).HasDefaultValue(false);

            builder.HasMany(x => x.PersonContactInfos)
                   .WithOne(s => s.Person)
                   .HasForeignKey(s => s.PersonId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
