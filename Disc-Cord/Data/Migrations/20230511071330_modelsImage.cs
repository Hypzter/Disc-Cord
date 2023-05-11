using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disc_Cord.Data.Migrations
{
    /// <inheritdoc />
    public partial class modelsImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "NewPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "NewPost");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Comment");
        }
    }
}
