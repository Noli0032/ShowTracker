using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserShowEntryCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShowEntries",
                table: "UserShowEntries");

            migrationBuilder.DropIndex(
                name: "IX_UserShowEntries_UserId",
                table: "UserShowEntries");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserShowEntries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShowEntries",
                table: "UserShowEntries",
                columns: new[] { "UserId", "TvMazeShowId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShowEntries",
                table: "UserShowEntries");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserShowEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShowEntries",
                table: "UserShowEntries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserShowEntries_UserId",
                table: "UserShowEntries",
                column: "UserId");
        }
    }
}
