using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilvagenumWebApp.Migrations
{
    /// <inheritdoc />
    public partial class OneWithMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_FatherId",
                table: "People");

            migrationBuilder.CreateIndex(
                name: "IX_People_FatherId",
                table: "People",
                column: "FatherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_People_FatherId",
                table: "People");

            migrationBuilder.CreateIndex(
                name: "IX_People_FatherId",
                table: "People",
                column: "FatherId",
                unique: true,
                filter: "[FatherId] IS NOT NULL");
        }
    }
}
