using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManyToMany.Core.Migrations
{
    /// <inheritdoc />
    public partial class Snapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "SnapShotEntwickler",
                table: "UserGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SnapShotGenres",
                table: "UserGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SnapShotSpielName",
                table: "UserGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnapShotEntwickler",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "SnapShotGenres",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "SnapShotSpielName",
                table: "UserGames");

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
