﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YourWheel.Domain;

#nullable disable

namespace YourWheel.Domain.Migrations
{
    [DbContext(typeof(YourWheelDbContext))]
    [Migration("20250405161217_FirstMigration")]
    partial class FirstMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CarUser", b =>
                {
                    b.Property<Guid>("Carid")
                        .HasColumnType("uuid")
                        .HasColumnName("carid");

                    b.Property<Guid>("Userid")
                        .HasColumnType("uuid")
                        .HasColumnName("userid");

                    b.HasKey("Carid", "Userid")
                        .HasName("CarUser_pkey");

                    b.HasIndex("Userid");

                    b.ToTable("CarUser", (string)null);
                });

            modelBuilder.Entity("YourWheel.Domain.Models.AppUser", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("appuserid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("CurrentLanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("currentlanguageid")
                        .HasDefaultValueSql("get_default_language()");

                    b.Property<bool?>("IsOnline")
                        .HasColumnType("boolean")
                        .HasColumnName("isonline");

                    b.Property<DateTime?>("LastConnected")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lastconnected")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime?>("LastDisconected")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lastdisconected");

                    b.Property<string>("LastIPAddress")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("lastipaddress");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("userid");

                    b.HasKey("AppUserId")
                        .HasName("AppUser_pkey");

                    b.HasIndex("CurrentLanguageId");

                    b.HasIndex("UserId");

                    b.ToTable("AppUser", (string)null);
                });

            modelBuilder.Entity("YourWheel.Domain.Models.Car", b =>
                {
                    b.Property<Guid>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("carid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Namemark")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("namemark");

                    b.HasKey("CarId")
                        .HasName("Car_pkey");

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("YourWheel.Domain.Models.Language", b =>
                {
                    b.Property<Guid>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("languageid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.HasKey("LanguageId")
                        .HasName("Language_pkey");

                    b.ToTable("Language", (string)null);
                });

            modelBuilder.Entity("YourWheel.Domain.Models.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("roleid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("name");

                    b.HasKey("RoleId")
                        .HasName("Role_pkey");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("YourWheel.Domain.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("userid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("login");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("phone");

                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("roleid")
                        .HasDefaultValueSql("get_default_role()");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("surname");

                    b.HasKey("UserId")
                        .HasName("User_pkey");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("CarUser", b =>
                {
                    b.HasOne("YourWheel.Domain.Models.Car", null)
                        .WithMany()
                        .HasForeignKey("Carid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_car");

                    b.HasOne("YourWheel.Domain.Models.User", null)
                        .WithMany()
                        .HasForeignKey("Userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user");
                });

            modelBuilder.Entity("YourWheel.Domain.Models.AppUser", b =>
                {
                    b.HasOne("YourWheel.Domain.Models.Language", "CurrentLanguage")
                        .WithMany("AppUsers")
                        .HasForeignKey("CurrentLanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_language_appuser");

                    b.HasOne("YourWheel.Domain.Models.User", "User")
                        .WithMany("AppUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_appuser");

                    b.Navigation("CurrentLanguage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YourWheel.Domain.Models.User", b =>
                {
                    b.HasOne("YourWheel.Domain.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_user");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("YourWheel.Domain.Models.Language", b =>
                {
                    b.Navigation("AppUsers");
                });

            modelBuilder.Entity("YourWheel.Domain.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("YourWheel.Domain.Models.User", b =>
                {
                    b.Navigation("AppUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
