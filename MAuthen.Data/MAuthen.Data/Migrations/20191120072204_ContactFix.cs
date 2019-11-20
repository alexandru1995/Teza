using Microsoft.EntityFrameworkCore.Migrations;

namespace MAuthen.Data.Migrations
{
    public partial class ContactFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactEmail_Contacts_ContactsId",
                table: "ContactEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactPhone_Contacts_ContactsId",
                table: "ContactPhone");

            migrationBuilder.RenameColumn(
                name: "ContactsId",
                table: "ContactPhone",
                newName: "ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactPhone_ContactsId",
                table: "ContactPhone",
                newName: "IX_ContactPhone_ContactId");

            migrationBuilder.RenameColumn(
                name: "ContactsId",
                table: "ContactEmail",
                newName: "ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactEmail_ContactsId",
                table: "ContactEmail",
                newName: "IX_ContactEmail_ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactEmail_Contacts_ContactId",
                table: "ContactEmail",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPhone_Contacts_ContactId",
                table: "ContactPhone",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactEmail_Contacts_ContactId",
                table: "ContactEmail");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactPhone_Contacts_ContactId",
                table: "ContactPhone");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "ContactPhone",
                newName: "ContactsId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactPhone_ContactId",
                table: "ContactPhone",
                newName: "IX_ContactPhone_ContactsId");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "ContactEmail",
                newName: "ContactsId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactEmail_ContactId",
                table: "ContactEmail",
                newName: "IX_ContactEmail_ContactsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactEmail_Contacts_ContactsId",
                table: "ContactEmail",
                column: "ContactsId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPhone_Contacts_ContactsId",
                table: "ContactPhone",
                column: "ContactsId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
