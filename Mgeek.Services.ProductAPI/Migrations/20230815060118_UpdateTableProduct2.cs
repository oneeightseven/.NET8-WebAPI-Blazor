using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mgeek.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableProduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThirdImageUrl",
                table: "Smartphones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdImageUrl",
                table: "Laptops",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThirdImageUrl",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "ThirdImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ThirdImageUrl",
                table: "Laptops");
        }
    }
}
