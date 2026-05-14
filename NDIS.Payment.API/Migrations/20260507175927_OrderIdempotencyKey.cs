using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDIS.Payment.API.Migrations
{
    /// <inheritdoc />
    public partial class OrderIdempotencyKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderIdempotencyKey",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIdempotencyKey",
                table: "Payments");
        }
    }
}
