using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAuthen.Data.Migrations
{
    public partial class ChangeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRoles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserServices");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "ContactEmail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    ContactsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactEmail_Contacts_ContactsId",
                        column: x => x.ContactsId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactPhone",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    ContactsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPhone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactPhone_Contacts_ContactsId",
                        column: x => x.ContactsId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserServiceRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServiceRoles", x => new { x.UserId, x.RoleId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_UserServiceRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServiceRoles_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServiceRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactEmail_ContactsId",
                table: "ContactEmail",
                column: "ContactsId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPhone_ContactsId",
                table: "ContactPhone",
                column: "ContactsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserServiceRoles_RoleId",
                table: "UserServiceRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserServiceRoles_ServiceId",
                table: "UserServiceRoles",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactEmail");

            migrationBuilder.DropTable(
                name: "ContactPhone");

            migrationBuilder.DropTable(
                name: "UserServiceRoles");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Contacts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceRoles",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(nullable: false),
                    IdService = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRoles", x => new { x.IdRole, x.IdService });
                    table.ForeignKey(
                        name: "FK_ServiceRoles_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRoles_Services_IdService",
                        column: x => x.IdService,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    IdRole = table.Column<Guid>(nullable: false),
                    IdUser = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.IdRole, x.IdUser });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserServices",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(nullable: false),
                    IdService = table.Column<Guid>(nullable: false),
                    Consent = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServices", x => new { x.IdUser, x.IdService });
                    table.ForeignKey(
                        name: "FK_UserServices_Services_IdService",
                        column: x => x.IdService,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserServices_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRoles_IdService",
                table: "ServiceRoles",
                column: "IdService");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_IdUser",
                table: "UserRoles",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_IdService",
                table: "UserServices",
                column: "IdService");
        }
    }
}
