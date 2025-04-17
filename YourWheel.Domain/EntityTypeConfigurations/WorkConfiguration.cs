namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class WorkConfiguration : IEntityTypeConfiguration<Work>
    {
        public void Configure(EntityTypeBuilder<Work> builder)
        {
            builder.HasKey(e => e.WorkId).HasName("Work_pkey");

            builder.ToTable("Work");

            builder.Property(e => e.WorkId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("workid");

            builder.Property(e => e.Discount)
                .HasDefaultValue(0)
                .HasColumnName("discount");

            builder.Property(e => e.MaterialId).HasColumnName("materialid");

            builder.Property(e => e.OrderId).HasColumnName("orderid");

            builder.Property(e => e.Price).HasColumnName("price");

            builder.Property(e => e.TypeWorkId).HasColumnName("typeworkid");

            builder.Property(e => e.WorkPlaceId).HasColumnName("workplaceid");

            builder.HasOne(d => d.Material).WithMany(p => p.Works)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_material_work");

            builder.HasOne(d => d.Order).WithMany(p => p.Works)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_work");

            builder.HasOne(d => d.Typework).WithMany(p => p.Works)
                .HasForeignKey(d => d.TypeWorkId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_typework_work");
        }
    }
}
