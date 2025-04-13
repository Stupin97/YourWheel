namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(e => e.SupplierId).HasName("Supplier_pkey");

            builder.ToTable("Supplier");

            builder.Property(e => e.SupplierId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("supplierid");

            builder.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");

            builder.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");

            builder.Property(e => e.Url)
                .HasMaxLength(1024)
                .HasColumnName("url");
        }
    }
}
