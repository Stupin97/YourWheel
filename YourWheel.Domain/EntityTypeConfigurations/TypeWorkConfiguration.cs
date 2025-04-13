namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class TypeWorkConfiguration : IEntityTypeConfiguration<TypeWork>
    {
        public void Configure(EntityTypeBuilder<TypeWork> builder)
        {
            builder.HasKey(e => e.TypeworkId).HasName("TypeWork_pkey");

            builder.ToTable("TypeWork");

            builder.Property(e => e.TypeworkId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("typeworkid");

            builder.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");

            builder.Property(e => e.Price).HasColumnName("price");
        }
    }
}
