using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManyToMany.Core.Migrations
{
    /// <inheritdoc />
    public partial class UserGameParameteres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGames",
                table: "UserGames");

            migrationBuilder.RenameColumn(
                name: "SnapShotSpielName",
                table: "UserGames",
                newName: "SpielName");

            migrationBuilder.RenameColumn(
                name: "SnapShotGenres",
                table: "UserGames",
                newName: "Genres");

            migrationBuilder.RenameColumn(
                name: "SnapShotEntwickler",
                table: "UserGames",
                newName: "Entwickler");

            migrationBuilder.AddColumn<int>(
                name: "GameLicenceId",
                table: "UserGames",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGames",
                table: "UserGames",
                column: "GameLicenceId");

            migrationBuilder.CreateTable(
                name: "GiftHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevieverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_PersonId",
                table: "UserGames",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGames",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_PersonId",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "GameLicenceId",
                table: "UserGames");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SpielName",
                table: "UserGames",
                newName: "SnapShotSpielName");

            migrationBuilder.RenameColumn(
                name: "Genres",
                table: "UserGames",
                newName: "SnapShotGenres");

            migrationBuilder.RenameColumn(
                name: "Entwickler",
                table: "UserGames",
                newName: "SnapShotEntwickler");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGames",
                table: "UserGames",
                columns: new[] { "PersonId", "GameId" });
        }
    }
}
