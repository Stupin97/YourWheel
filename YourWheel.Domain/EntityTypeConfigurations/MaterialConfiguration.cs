namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using YourWheel.Domain.Models;

    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(e => e.MaterialId).HasName("Material_pkey");

            builder.ToTable("Material");

            builder.Property(e => e.MaterialId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("materialid");

            builder.Property(e => e.ColorId).HasColumnName("colorid");

            builder.Property(e => e.FabricatorId).HasColumnName("fabricatorid");

            builder.Property(e => e.Name)
                .HasMaxLength(512)
                .HasColumnName("name");

            builder.Property(e => e.Price).HasColumnName("price");

            builder.Property(e => e.SupplierId).HasColumnName("supplierid");

            builder.HasOne(d => d.Color).WithMany(p => p.Materials)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("fk_color_material");

            builder.HasOne(d => d.Fabricator).WithMany(p => p.Materials)
                .HasForeignKey(d => d.FabricatorId)
                .HasConstraintName("fk_fubricator_material");

            builder.HasOne(d => d.Supplier).WithMany(p => p.Materials)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("fk_supplier_material");
        }
    }
}
