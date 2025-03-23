using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
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

    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="surname">Фамилия</param>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <param name="phone">Телефон</param>
    /// <param name="email">Эл. почта</param>
    /// <returns>Набор данных с Users</returns>
    [DbFunction("set_user_instance", "public")]
    public IQueryable<User> SetUserInstance(string name, string surname, string login, string password, string phone, string email) => FromExpression(() => this.SetUserInstance(name, surname, login, password, phone, email));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YourWheelDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
