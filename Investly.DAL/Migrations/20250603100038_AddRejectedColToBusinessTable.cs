using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectedColToBusinessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectedReason",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedReason",
                table: "Business");
        }
    }
}
