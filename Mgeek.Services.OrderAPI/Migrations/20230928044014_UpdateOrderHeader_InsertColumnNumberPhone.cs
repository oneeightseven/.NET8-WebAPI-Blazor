using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mgeek.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderHeader_InsertColumnNumberPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberPhone",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberPhone",
                table: "OrderHeaders");
        }
    }
}
