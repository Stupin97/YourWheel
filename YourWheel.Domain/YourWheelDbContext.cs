using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YourWheel.Domain.Models;

namespace YourWheel.Domain;

public class YourWheelDbContext : DbContext
{
    public YourWheelDbContext()
    {
    }

    public YourWheelDbContext(DbContextOptions<YourWheelDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<Language> Languages { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Получение (Создание если отсутствует запись) AppUsers по userId пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Набор данных с AppUsers</returns>
    [DbFunction("set_and_get_upp_user_instance", "public")]
    public IQueryable<AppUser> SetAndGetUppUserInstance(Guid userId) => FromExpression(() => this.SetAndGetUppUserInstance(userId));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YourWheelDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
