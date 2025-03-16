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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YourWheelDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
