using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Identity.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration_UserIdentityDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "gx",
                newName: "AspNetUserTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "gx",
                newName: "AspNetUsers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "gx",
                newName: "AspNetUserRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "gx",
                newName: "AspNetUserLogins",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "gx",
                newName: "AspNetUserClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "gx",
                newName: "AspNetRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "gx",
                newName: "AspNetRoleClaims",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                newName: "AspNetUserTokens",
                newSchema: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "dbo",
                newName: "AspNetUsers",
                newSchema: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                newName: "AspNetUserRoles",
                newSchema: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                newName: "AspNetUserLogins",
                newSchema: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                newName: "AspNetUserClaims",
                newSchema: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "dbo",
                newName: "AspNetRoles",
                newSchema: "gx");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                newName: "AspNetRoleClaims",
                newSchema: "gx");
        }
    }
}
