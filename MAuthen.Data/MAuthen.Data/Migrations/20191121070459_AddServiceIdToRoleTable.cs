using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAuthen.Data.Migrations
{
    public partial class AddServiceIdToRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "Roles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Roles");
        }
    }
}
