using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilvagenumWebApp.Migrations
{
    /// <inheritdoc />
    public partial class childrenlistunmapped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_People_PersonId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_PersonId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "People");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "People",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "People",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_PersonId",
                table: "People",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_People_PersonId",
                table: "People",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id");
        }
    }
}
