using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDIS.Order.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderEventDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EventType",
                table: "OrderEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "EventStatus",
                table: "OrderEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "OrderEvents",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "OrderEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAt",
                table: "OrderEvents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "OrderEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderEvents_EventStatus",
                table: "OrderEvents",
                column: "EventStatus");

            migrationBuilder.CreateIndex(
                name: "IX_OrderEvents_EventTimestamp",
                table: "OrderEvents",
                column: "EventTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderEvents_EventStatus",
                table: "OrderEvents");

            migrationBuilder.DropIndex(
                name: "IX_OrderEvents_EventTimestamp",
                table: "OrderEvents");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "OrderEvents");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "OrderEvents");

            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                table: "OrderEvents");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "OrderEvents");

            migrationBuilder.AlterColumn<string>(
                name: "EventType",
                table: "OrderEvents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EventStatus",
                table: "OrderEvents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
