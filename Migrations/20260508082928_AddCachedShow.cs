using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class AddCachedShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CachedShow",
                columns: table => new
                {
                    TvMazeShowId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    LastSyncedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedShow", x => x.TvMazeShowId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShowEntries_TvMazeShowId",
                table: "UserShowEntries",
                column: "TvMazeShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserShowEntries_CachedShow_TvMazeShowId",
                table: "UserShowEntries",
                column: "TvMazeShowId",
                principalTable: "CachedShow",
                principalColumn: "TvMazeShowId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShowEntries_CachedShow_TvMazeShowId",
                table: "UserShowEntries");

            migrationBuilder.DropTable(
                name: "CachedShow");

            migrationBuilder.DropIndex(
                name: "IX_UserShowEntries_TvMazeShowId",
                table: "UserShowEntries");
        }
    }
}
