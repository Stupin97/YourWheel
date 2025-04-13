namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class MasterConfiguration : IEntityTypeConfiguration<Master>
    {
        public void Configure(EntityTypeBuilder<Master> builder)
        {
            builder.HasKey(e => e.MasterId).HasName("Master_pkey");

            builder.ToTable("Master");

            builder.Property(e => e.MasterId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("masterid");

            builder.Property(e => e.PositionId).HasColumnName("positionid");

            builder.Property(e => e.UserId).HasColumnName("userid");

            builder.Property(e => e.WorkExperienceDate).HasColumnName("workexperiencedate");

            builder.HasOne(d => d.Position).WithMany(p => p.Masters)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_master_position");

            builder.HasOne(d => d.User).WithMany(p => p.Masters)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_master");

            builder.HasMany(d => d.Orders).WithMany(p => p.Masters)
                .UsingEntity<Dictionary<string, object>>(
                    "MasterOrder",
                    r => r.HasOne<Order>().WithMany()
                        .HasForeignKey("Orderid")
                        .HasConstraintName("fk_order"),
                    l => l.HasOne<Master>().WithMany()
                        .HasForeignKey("Masterid")
                        .HasConstraintName("fk_mater"),
                    j =>
                    {
                        j.HasKey("Masterid", "Orderid").HasName("MasterOrder_pkey");
                        j.ToTable("MasterOrder");
                        j.IndexerProperty<Guid>("Masterid").HasColumnName("masterid");
                        j.IndexerProperty<Guid>("Orderid").HasColumnName("orderid");
                    });
        }
    }
}
