using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAgentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "agents");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "agents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "agents");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "agents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
