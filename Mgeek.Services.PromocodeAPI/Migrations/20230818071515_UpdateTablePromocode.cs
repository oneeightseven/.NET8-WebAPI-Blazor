using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mgeek.Services.PromocodeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablePromocode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinAmount",
                table: "Promocodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinAmount",
                table: "Promocodes");
        }
    }
}
