using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDIS.Payment.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessedWebhookEvent",
                table: "ProcessedWebhookEvent");

            migrationBuilder.RenameTable(
                name: "ProcessedWebhookEvent",
                newName: "ProcessedWebhookEvents");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessedWebhookEvent_Provider_EventId",
                table: "ProcessedWebhookEvents",
                newName: "IX_ProcessedWebhookEvents_Provider_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessedWebhookEvents",
                table: "ProcessedWebhookEvents",
                column: "ProcessedWebhookEventId");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    OutboxMessageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.OutboxMessageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedAt",
                table: "OutboxMessages",
                column: "ProcessedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessedWebhookEvents",
                table: "ProcessedWebhookEvents");

            migrationBuilder.RenameTable(
                name: "ProcessedWebhookEvents",
                newName: "ProcessedWebhookEvent");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessedWebhookEvents_Provider_EventId",
                table: "ProcessedWebhookEvent",
                newName: "IX_ProcessedWebhookEvent_Provider_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessedWebhookEvent",
                table: "ProcessedWebhookEvent",
                column: "ProcessedWebhookEventId");
        }
    }
}
