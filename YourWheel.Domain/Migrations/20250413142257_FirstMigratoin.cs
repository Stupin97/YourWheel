using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourWheel.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigratoin : Migration
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
                name: "Color",
                columns: table => new
                {
                    colorid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Color_pkey", x => x.colorid);
                });

            migrationBuilder.CreateTable(
                name: "Fabricator",
                columns: table => new
                {
                    fabricatorid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    country = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Fabricator_pkey", x => x.fabricatorid);
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
                name: "PlaceOrder",
                columns: table => new
                {
                    placeorderid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaceOrder_pkey", x => x.placeorderid);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    positionid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Position_pkey", x => x.positionid);
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
                name: "Status",
                columns: table => new
                {
                    statusid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Status_pkey", x => x.statusid);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    supplierid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Supplier_pkey", x => x.supplierid);
                });

            migrationBuilder.CreateTable(
                name: "TypeWork",
                columns: table => new
                {
                    typeworkid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TypeWork_pkey", x => x.typeworkid);
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
                name: "Material",
                columns: table => new
                {
                    materialid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: true),
                    supplierid = table.Column<Guid>(type: "uuid", nullable: false),
                    fabricatorid = table.Column<Guid>(type: "uuid", nullable: false),
                    colorid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Material_pkey", x => x.materialid);
                    table.ForeignKey(
                        name: "fk_color_material",
                        column: x => x.colorid,
                        principalTable: "Color",
                        principalColumn: "colorid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_fubricator_material",
                        column: x => x.fabricatorid,
                        principalTable: "Fabricator",
                        principalColumn: "fabricatorid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_supplier_material",
                        column: x => x.supplierid,
                        principalTable: "Supplier",
                        principalColumn: "supplierid",
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

            migrationBuilder.CreateTable(
                name: "Master",
                columns: table => new
                {
                    masterid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    workexperiencedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    positionid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Master_pkey", x => x.masterid);
                    table.ForeignKey(
                        name: "fk_master_position",
                        column: x => x.positionid,
                        principalTable: "Position",
                        principalColumn: "positionid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_master",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    orderid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    dateorder = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    statusid = table.Column<Guid>(type: "uuid", nullable: false),
                    discount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    dateexecute = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    placeorderid = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    dateend = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    majorid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_pkey", x => x.orderid);
                    table.ForeignKey(
                        name: "fk_major_order",
                        column: x => x.majorid,
                        principalTable: "Order",
                        principalColumn: "orderid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_placeorder_order",
                        column: x => x.placeorderid,
                        principalTable: "PlaceOrder",
                        principalColumn: "placeorderid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_status_order",
                        column: x => x.statusid,
                        principalTable: "Status",
                        principalColumn: "statusid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_order",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterOrder",
                columns: table => new
                {
                    masterid = table.Column<Guid>(type: "uuid", nullable: false),
                    orderid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MasterOrder_pkey", x => new { x.masterid, x.orderid });
                    table.ForeignKey(
                        name: "fk_mater",
                        column: x => x.masterid,
                        principalTable: "Master",
                        principalColumn: "masterid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order",
                        column: x => x.orderid,
                        principalTable: "Order",
                        principalColumn: "orderid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Work",
                columns: table => new
                {
                    workid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    orderid = table.Column<Guid>(type: "uuid", nullable: false),
                    productid = table.Column<Guid>(type: "uuid", nullable: true),
                    typeworkid = table.Column<Guid>(type: "uuid", nullable: true),
                    materialid = table.Column<Guid>(type: "uuid", nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    discount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    workplaceid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Work_pkey", x => x.workid);
                    table.ForeignKey(
                        name: "fk_material_work",
                        column: x => x.materialid,
                        principalTable: "Material",
                        principalColumn: "materialid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_work",
                        column: x => x.orderid,
                        principalTable: "Order",
                        principalColumn: "orderid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_typework_work",
                        column: x => x.typeworkid,
                        principalTable: "TypeWork",
                        principalColumn: "typeworkid",
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
                name: "IX_Master_positionid",
                table: "Master",
                column: "positionid");

            migrationBuilder.CreateIndex(
                name: "IX_Master_userid",
                table: "Master",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_MasterOrder_orderid",
                table: "MasterOrder",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_colorid",
                table: "Material",
                column: "colorid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_fabricatorid",
                table: "Material",
                column: "fabricatorid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_supplierid",
                table: "Material",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_majorid",
                table: "Order",
                column: "majorid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_placeorderid",
                table: "Order",
                column: "placeorderid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_statusid",
                table: "Order",
                column: "statusid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_userid",
                table: "Order",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_User_roleid",
                table: "User",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_Work_materialid",
                table: "Work",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "IX_Work_orderid",
                table: "Work",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "IX_Work_typeworkid",
                table: "Work",
                column: "typeworkid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "CarUser");

            migrationBuilder.DropTable(
                name: "MasterOrder");

            migrationBuilder.DropTable(
                name: "Work");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Master");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "TypeWork");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Fabricator");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "PlaceOrder");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
