using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonolithicMultimedia.Migrations
{
    /// <inheritdoc />
    public partial class AddHashtags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hashtag",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hashtag",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hashtag",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Hashtag",
                table: "Images");
        }
    }
}
