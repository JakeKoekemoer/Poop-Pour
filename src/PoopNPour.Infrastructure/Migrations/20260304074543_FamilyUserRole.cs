using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoopNPour.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FamilyUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "FamilyUsers",
                type: "int",
                nullable: false,
                defaultValue: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "FamilyUsers");
        }
    }
}
