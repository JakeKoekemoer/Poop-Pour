using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoopNPour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MedicineLogBaseAuditableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MedicineLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "MedicineLogs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "MedicineLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedOn",
                table: "MedicineLogs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MedicineLogs");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MedicineLogs");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "MedicineLogs");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "MedicineLogs");
        }
    }
}
