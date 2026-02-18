using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NDISS.NotificationService.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedNotificationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "NotificationTypeName" },
                values: new object[,]
                {
                    { "1", "SMS" },
                    { "2", "Email" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: "2");
        }
    }
}
