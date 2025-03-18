using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(e => e.LanguageId).HasName("Language_pkey");

            builder.ToTable("Language");

            builder.Property(e => e.LanguageId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("languageid")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        }
    }
}
