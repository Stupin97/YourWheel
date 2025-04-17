namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.OrderId).HasName("Order_pkey");

            builder.ToTable("Order");

            builder.Property(e => e.OrderId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("orderid");

            builder.Property(e => e.DateEnd).HasColumnName("dateend");

            builder.Property(e => e.DateExecute).HasColumnName("dateexecute");

            builder.Property(e => e.DateOrder)
                .HasDefaultValueSql("now()")
                .HasColumnName("dateorder");

            builder.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasColumnName("description");

            builder.Property(e => e.Discount)
                .HasDefaultValue(0)
                .HasColumnName("discount");

            builder.Property(e => e.MajorId).HasColumnName("majorid");

            builder.Property(e => e.PlaceOrderId).HasColumnName("placeorderid");

            builder.Property(e => e.StatusId).HasColumnName("statusid");

            builder.Property(e => e.UserId).HasColumnName("userid");

            builder.HasOne(d => d.Major).WithMany(p => p.InverseMajor)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_major_order");

            builder.HasOne(d => d.PlaceOrder).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PlaceOrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_placeorder_order");

            builder.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("fk_status_order");

            builder.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_order");
        }
    }
}
