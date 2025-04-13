namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(e => e.ColorId).HasName("Color_pkey");

            builder.ToTable("Color");

            builder.Property(e => e.ColorId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("colorid");

            builder.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        }
    }
}
