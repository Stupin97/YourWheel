namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(e => e.StatusId).HasName("Status_pkey");

            builder.ToTable("Status");

            builder.Property(e => e.StatusId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("statusid");

            builder.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        }
    }
}
