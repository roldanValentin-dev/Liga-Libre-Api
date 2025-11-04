using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaLibre.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renombreEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "Referee",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "Match",
                newName: "UpdateAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Referee",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Match",
                newName: "LastUpdatedAt");
        }
    }
}
