using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesToBusinessTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "Business");
        }
    }
}
