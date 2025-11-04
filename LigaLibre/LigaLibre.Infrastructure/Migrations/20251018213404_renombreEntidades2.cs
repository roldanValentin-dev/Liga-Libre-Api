using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaLibre.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renombreEntidades2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FistName",
                table: "Referee",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Referee",
                newName: "FistName");
        }
    }
}
