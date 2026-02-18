using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDISS.UserRebate.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessTokenToUserRebate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "UserRebates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "UserRebates");
        }
    }
}
