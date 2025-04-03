namespace YourWheel.Domain.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using YourWheel.Domain.Models;

    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(e => e.CarId).HasName("Car_pkey");

            builder.ToTable("Car");

            builder.Property(e => e.CarId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("carid");

            builder.Property(e => e.Namemark)
                    .HasMaxLength(50)
                    .HasColumnName("namemark");


            builder.HasMany(d => d.Users).WithMany(p => p.Cars)
                .UsingEntity<Dictionary<string, object>>(
                    "CarUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .HasConstraintName("fk_user"),
                    l => l.HasOne<Car>().WithMany()
                        .HasForeignKey("Carid")
                        .HasConstraintName("fk_car"),
                    j =>
                    {
                        j.HasKey("Carid", "Userid").HasName("CarUser_pkey");
                        j.ToTable("CarUser");
                        j.IndexerProperty<Guid>("Carid").HasColumnName("carid");
                        j.IndexerProperty<Guid>("Userid").HasColumnName("userid");
                    });
        }
    }
}
