using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addInterestedBusinessStagesFieldToInvestorTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InterestedBusinessStages",
                table: "Investors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestedBusinessStages",
                table: "Investors");
        }
    }
}
