namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class FabricatorConfiguration : IEntityTypeConfiguration<Fabricator>
    {
        public void Configure(EntityTypeBuilder<Fabricator> builder)
        {
            builder.HasKey(e => e.FabricatorId).HasName("Fabricator_pkey");

            builder.ToTable("Fabricator");

            builder.Property(e => e.FabricatorId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("fabricatorid");

            builder.Property(e => e.Country)
                .HasMaxLength(256)
                .HasColumnName("country");

            builder.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        }
    }
}
