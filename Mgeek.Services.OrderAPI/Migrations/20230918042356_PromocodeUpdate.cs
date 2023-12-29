using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mgeek.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class PromocodeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Promocode",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Promocode",
                table: "OrderHeaders");
        }
    }
}
