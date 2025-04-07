using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourWheel.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    carid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    namemark = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Car_pkey", x => x.carid);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    languageid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Language_pkey", x => x.languageid);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    roleid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_pkey", x => x.roleid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    login = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    password = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    roleid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "get_default_role()"),
                    email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.userid);
                    table.ForeignKey(
                        name: "fk_role_user",
                        column: x => x.roleid,
                        principalTable: "Role",
                        principalColumn: "roleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    appuserid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    lastipaddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    isonline = table.Column<bool>(type: "boolean", nullable: true),
                    lastconnected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    lastdisconected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    currentlanguageid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "get_default_language()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("AppUser_pkey", x => x.appuserid);
                    table.ForeignKey(
                        name: "fk_language_appuser",
                        column: x => x.currentlanguageid,
                        principalTable: "Language",
                        principalColumn: "languageid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_appuser",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarUser",
                columns: table => new
                {
                    carid = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CarUser_pkey", x => new { x.carid, x.userid });
                    table.ForeignKey(
                        name: "fk_car",
                        column: x => x.carid,
                        principalTable: "Car",
                        principalColumn: "carid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_currentlanguageid",
                table: "AppUser",
                column: "currentlanguageid");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_userid",
                table: "AppUser",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_CarUser_userid",
                table: "CarUser",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_User_roleid",
                table: "User",
                column: "roleid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "CarUser");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
