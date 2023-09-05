using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilvagenumWebApp.Migrations
{
    /// <inheritdoc />
    public partial class OneWithMany2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_MotherId",
                table: "People");

            migrationBuilder.CreateIndex(
                name: "IX_People_MotherId",
                table: "People",
                column: "MotherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_MotherId",
                table: "People");

            migrationBuilder.CreateIndex(
                name: "IX_People_MotherId",
                table: "People",
                column: "MotherId",
                unique: true,
                filter: "[MotherId] IS NOT NULL");
        }
    }
}
