using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LigaLibre.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameClubProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroPartners",
                table: "Club");

            migrationBuilder.RenameColumn(
                name: "StadiumBase",
                table: "Club",
                newName: "StadiumName");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Club",
                newName: "Address");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPartners",
                table: "Club",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfPartners",
                table: "Club");

            migrationBuilder.RenameColumn(
                name: "StadiumName",
                table: "Club",
                newName: "StadiumBase");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Club",
                newName: "Adress");

            migrationBuilder.AddColumn<int>(
                name: "NumeroPartners",
                table: "Club",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
