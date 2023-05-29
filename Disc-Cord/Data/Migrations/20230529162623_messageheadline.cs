using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disc_Cord.Data.Migrations
{
    /// <inheritdoc />
    public partial class messageheadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Headline",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Headline",
                table: "Messages");
        }
    }
}
