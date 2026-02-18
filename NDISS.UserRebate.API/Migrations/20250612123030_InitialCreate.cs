using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDISS.UserRebate.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RebateEventTypes",
                columns: table => new
                {
                    RebateEventTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RebateEventTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RebateEventTypes", x => x.RebateEventTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserRebates",
                columns: table => new
                {
                    UserRebateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RebateRate = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRebates", x => x.UserRebateId);
                });

            migrationBuilder.CreateTable(
                name: "RebateEvents",
                columns: table => new
                {
                    RebateEventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RebateEventTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserRebateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SyncStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RebateEvents", x => x.RebateEventId);
                    table.ForeignKey(
                        name: "FK_RebateEvents_RebateEventTypes_RebateEventTypeId",
                        column: x => x.RebateEventTypeId,
                        principalTable: "RebateEventTypes",
                        principalColumn: "RebateEventTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RebateEvents_UserRebates_UserRebateId",
                        column: x => x.UserRebateId,
                        principalTable: "UserRebates",
                        principalColumn: "UserRebateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RebateRetrySchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RebateEventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    RetryReason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RebateRetrySchedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_RebateRetrySchedules_RebateEvents_RebateEventId",
                        column: x => x.RebateEventId,
                        principalTable: "RebateEvents",
                        principalColumn: "RebateEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RebateEvents_RebateEventTypeId",
                table: "RebateEvents",
                column: "RebateEventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RebateEvents_UserRebateId",
                table: "RebateEvents",
                column: "UserRebateId");

            migrationBuilder.CreateIndex(
                name: "IX_RebateRetrySchedules_RebateEventId",
                table: "RebateRetrySchedules",
                column: "RebateEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RebateRetrySchedules");

            migrationBuilder.DropTable(
                name: "RebateEvents");

            migrationBuilder.DropTable(
                name: "RebateEventTypes");

            migrationBuilder.DropTable(
                name: "UserRebates");
        }
    }
}
