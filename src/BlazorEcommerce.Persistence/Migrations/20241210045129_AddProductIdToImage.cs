using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEcommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductIdToImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                schema: "gx",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                schema: "gx",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                schema: "gx",
                table: "Images",
                column: "ProductId",
                principalSchema: "gx",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_ProductId",
                schema: "gx",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                schema: "gx",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_ProductId",
                schema: "gx",
                table: "Images",
                column: "ProductId",
                principalSchema: "gx",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
