using Microsoft.EntityFrameworkCore.Migrations;

namespace MAuthen.Data.Migrations
{
    public partial class TestManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRole_Roles_IdRole",
                table: "ServiceRole");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRole_Services_IdService",
                table: "ServiceRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Roles_IdRole",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_IdUser",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserService_Services_IdService",
                table: "UserService");

            migrationBuilder.DropForeignKey(
                name: "FK_UserService_Users_IdUser",
                table: "UserService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserService",
                table: "UserService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceRole",
                table: "ServiceRole");

            migrationBuilder.RenameTable(
                name: "UserService",
                newName: "UserServices");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "ServiceRole",
                newName: "ServiceRoles");

            migrationBuilder.RenameIndex(
                name: "IX_UserService_IdService",
                table: "UserServices",
                newName: "IX_UserServices_IdService");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_IdUser",
                table: "UserRoles",
                newName: "IX_UserRoles_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRole_IdService",
                table: "ServiceRoles",
                newName: "IX_ServiceRoles_IdService");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices",
                columns: new[] { "IdUser", "IdService" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "IdRole", "IdUser" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceRoles",
                table: "ServiceRoles",
                columns: new[] { "IdRole", "IdService" });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRoles_Roles_IdRole",
                table: "ServiceRoles",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRoles_Services_IdService",
                table: "ServiceRoles",
                column: "IdService",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_IdRole",
                table: "UserRoles",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_IdUser",
                table: "UserRoles",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserServices_Services_IdService",
                table: "UserServices",
                column: "IdService",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserServices_Users_IdUser",
                table: "UserServices",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRoles_Roles_IdRole",
                table: "ServiceRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRoles_Services_IdService",
                table: "ServiceRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_IdRole",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_IdUser",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserServices_Services_IdService",
                table: "UserServices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserServices_Users_IdUser",
                table: "UserServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceRoles",
                table: "ServiceRoles");

            migrationBuilder.RenameTable(
                name: "UserServices",
                newName: "UserService");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRole");

            migrationBuilder.RenameTable(
                name: "ServiceRoles",
                newName: "ServiceRole");

            migrationBuilder.RenameIndex(
                name: "IX_UserServices_IdService",
                table: "UserService",
                newName: "IX_UserService_IdService");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_IdUser",
                table: "UserRole",
                newName: "IX_UserRole_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRoles_IdService",
                table: "ServiceRole",
                newName: "IX_ServiceRole_IdService");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserService",
                table: "UserService",
                columns: new[] { "IdUser", "IdService" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                columns: new[] { "IdRole", "IdUser" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceRole",
                table: "ServiceRole",
                columns: new[] { "IdRole", "IdService" });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRole_Roles_IdRole",
                table: "ServiceRole",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRole_Services_IdService",
                table: "ServiceRole",
                column: "IdService",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Roles_IdRole",
                table: "UserRole",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_IdUser",
                table: "UserRole",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserService_Services_IdService",
                table: "UserService",
                column: "IdService",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserService_Users_IdUser",
                table: "UserService",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
