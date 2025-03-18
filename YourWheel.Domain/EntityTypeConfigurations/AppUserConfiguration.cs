namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(e => e.AppuserId).HasName("AppUser_pkey");

            builder.ToTable("AppUser");

            builder.Property(e => e.AppuserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("appuserid")
                .IsRequired();

            builder.Property(e => e.CurrentlanguageId)
                .HasColumnName("currentlanguageid")
                .IsRequired();

            builder.Property(e => e.Isonline)
                .HasColumnName("isonline");

            builder.Property(e => e.Lastconnected)
                .HasColumnName("lastconnected");

            builder.Property(e => e.Lastdisconected)
                .HasColumnName("lastdisconected");

            builder.Property(e => e.Lastipaddress)
                .HasMaxLength(64)
                .HasColumnName("lastipaddress");

            builder.Property(e => e.UserId)
                .HasColumnName("userid")
                .IsRequired();

            builder.HasOne(d => d.Currentlanguage)
                .WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.CurrentlanguageId)
                .HasConstraintName("fk_language_appuser");

            builder.HasOne(d => d.User)
                .WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_appuser");
        }
    }
}
