namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(e => e.AppUserId).HasName("AppUser_pkey");

            builder.ToTable("AppUser");

            builder.Property(e => e.AppUserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("appuserid")
                .IsRequired();

            builder.Property(e => e.CurrentLanguageId)
                .HasDefaultValueSql("get_default_language()")
                .HasColumnName("currentlanguageid")
                .IsRequired();

            builder.Property(e => e.IsOnline)
                .HasColumnName("isonline");

            builder.Property(e => e.LastConnected)
                .HasDefaultValueSql("now()")
                .HasColumnName("lastconnected");

            builder.Property(e => e.LastDisconected)
                .HasColumnName("lastdisconected");

            builder.Property(e => e.LastIPAddress)
                .HasMaxLength(64)
                .HasColumnName("lastipaddress");

            builder.Property(e => e.UserId)
                .HasColumnName("userid")
                .IsRequired();

            builder.HasOne(d => d.CurrentLanguage)
                .WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.CurrentLanguageId)
                .HasConstraintName("fk_language_appuser");

            builder.HasOne(d => d.User)
                .WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_appuser");
        }
    }
}
