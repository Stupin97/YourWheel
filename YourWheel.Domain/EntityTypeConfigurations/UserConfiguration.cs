namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    /// <summary>
    /// Группирование конфигурации для User
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId).HasName("User_pkey");

            builder.ToTable("User");

            builder.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("userid")
                .IsRequired(); 

            builder.Property(e => e.Login)
                .HasMaxLength(128)
                .HasColumnName("login")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            builder.Property(e => e.Password)
                .HasMaxLength(1024)
                .HasColumnName("password")
                .IsRequired();

            builder.Property(e => e.Phone)
                .HasMaxLength(32)
                .HasColumnName("phone");

            builder.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email");

            builder.Property(e => e.RoleId)
                .HasDefaultValueSql("get_default_role()")
                .HasColumnName("roleid")
                .IsRequired();

            builder.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_role_user");
        }
    }
}
