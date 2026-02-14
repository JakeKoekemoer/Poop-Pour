using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoopNPour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FeedLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedLogs",
                columns: table => new
                {
                    FeedLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DependentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedType = table.Column<int>(type: "int", nullable: false),
                    TimeFed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MililitersFed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedLogs", x => x.FeedLogId);
                    table.ForeignKey(
                        name: "FK_FeedLogs_Dependents_DependentId",
                        column: x => x.DependentId,
                        principalTable: "Dependents",
                        principalColumn: "DependentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedLogs_DependentId",
                table: "FeedLogs",
                column: "DependentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedLogs");
        }
    }
}
