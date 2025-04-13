namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasKey(e => e.PositionId).HasName("Position_pkey");

            builder.ToTable("Position");

            builder.Property(e => e.PositionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("positionid");

            builder.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
        }
    }
}
