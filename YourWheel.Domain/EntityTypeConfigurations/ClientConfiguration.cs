namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    /// <summary>
    /// Группирование конфигурации для Client
    /// </summary>
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(e => e.ClientId).HasName("Client_pkey");

            builder.ToTable("Client");

            builder.Property(e => e.ClientId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("clientid")
                .IsRequired();

            builder.Property(e => e.Login)
                .HasMaxLength(128)
                .HasColumnName("login")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            builder.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password")
                .IsRequired();

            builder.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");
        }
    }
}
