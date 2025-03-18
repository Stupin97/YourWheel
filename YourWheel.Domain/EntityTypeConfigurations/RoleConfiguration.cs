namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    /// <summary>
    /// Группирование конфигурации для Role
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.RoleId).HasName("Role_pkey");

            builder.ToTable("Role");

            builder.Property(e => e.RoleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("roleid")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        }
    }
}
