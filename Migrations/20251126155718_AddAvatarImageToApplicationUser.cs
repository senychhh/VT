using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kolbasin_lab1.Migrations
{
    /// <inheritdoc />
    public partial class AddAvatarImageToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarImage",
                table: "AspNetUsers",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImage",
                table: "AspNetUsers");
        }
    }
}
