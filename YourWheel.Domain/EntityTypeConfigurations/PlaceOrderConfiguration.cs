namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class PlaceOrderConfiguration : IEntityTypeConfiguration<PlaceOrder>
    {
        public void Configure(EntityTypeBuilder<PlaceOrder> builder)
        {
            builder.HasKey(e => e.PlaceorderId).HasName("PlaceOrder_pkey");

            builder.ToTable("PlaceOrder");

            builder.Property(e => e.PlaceorderId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("placeorderid");

            builder.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
        }
    }
}
