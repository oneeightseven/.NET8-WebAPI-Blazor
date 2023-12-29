using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mgeek.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Smartphones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondImageUrl",
                table: "Smartphones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Laptops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondImageUrl",
                table: "Laptops",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "SecondImageUrl",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SecondImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "SecondImageUrl",
                table: "Laptops");
        }
    }
}
