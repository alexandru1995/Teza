using Microsoft.EntityFrameworkCore.Migrations;

namespace MAuthen.Data.Migrations
{
    public partial class AddDomainToService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "Services",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_Domain",
                table: "Services",
                column: "Domain",
                unique: true,
                filter: "[Domain] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_Domain",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "Services");
        }
    }
}
