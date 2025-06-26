using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToBusinessIdeas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Business",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Business",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovernmentId",
                table: "Business",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Business_CityId",
                table: "Business",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_GovernmentId",
                table: "Business",
                column: "GovernmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_Cities_CityId",
                table: "Business",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Business_Governments_GovernmentId",
                table: "Business",
                column: "GovernmentId",
                principalTable: "Governments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Business_Cities_CityId",
                table: "Business");

            migrationBuilder.DropForeignKey(
                name: "FK_Business_Governments_GovernmentId",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_Business_CityId",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_Business_GovernmentId",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "GovernmentId",
                table: "Business");
        }
    }
}
