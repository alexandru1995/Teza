using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAuthen.Data.Migrations
{
    public partial class ModifiedServiceTableForOIC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_Domain",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Domain",
                table: "Services",
                newName: "ServicePassword");

            migrationBuilder.AlterColumn<string>(
                name: "ServicePassword",
                table: "Services",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issuer",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoutUrl",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpirationTime",
                table: "Services",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Services_Issuer",
                table: "Services",
                column: "Issuer",
                unique: true,
                filter: "[Issuer] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_Issuer",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "LogoutUrl",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TokenExpirationTime",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ServicePassword",
                table: "Services",
                newName: "Domain");

            migrationBuilder.AlterColumn<string>(
                name: "Domain",
                table: "Services",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_Domain",
                table: "Services",
                column: "Domain",
                unique: true,
                filter: "[Domain] IS NOT NULL");
        }
    }
}
