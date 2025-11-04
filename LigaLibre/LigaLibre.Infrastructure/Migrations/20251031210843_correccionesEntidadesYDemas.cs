using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaLibre.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class correccionesEntidadesYDemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "height",
                table: "Player",
                newName: "Height");

            migrationBuilder.RenameColumn(
                name: "JerseuNumber",
                table: "Player",
                newName: "JerseyNumber");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "Match",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAT",
                table: "Club",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Player",
                newName: "height");

            migrationBuilder.RenameColumn(
                name: "JerseyNumber",
                table: "Player",
                newName: "JerseuNumber");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Match",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Club",
                newName: "CreatedAT");
        }
    }
}
