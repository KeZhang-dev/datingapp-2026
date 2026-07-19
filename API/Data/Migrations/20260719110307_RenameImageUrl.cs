using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Imageurl",
                table: "Users",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Imageurl",
                table: "Members",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Users",
                newName: "Imageurl");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Members",
                newName: "Imageurl");
        }
    }
}
