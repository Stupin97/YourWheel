using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YourWheel.Domain.Models;

namespace YourWheel.Domain;

public partial class YourWheelDbContext : DbContext
{
    public YourWheelDbContext()
    {
    }

    public YourWheelDbContext(DbContextOptions<YourWheelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=YourWheel;Username=postgres;Password=11111");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Clientid).HasName("Client_pkey");

            entity.ToTable("Client");

            entity.Property(e => e.Clientid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("clientid");
            entity.Property(e => e.Login)
                .HasMaxLength(128)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
