using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonolithicMultimedia.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Videos",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Images",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Videos",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Images",
                newName: "Name");
        }
    }
}
