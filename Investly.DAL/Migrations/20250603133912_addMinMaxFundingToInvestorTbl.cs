using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addMinMaxFundingToInvestorTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxFunding",
                table: "Investors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinFunding",
                table: "Investors",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFunding",
                table: "Investors");

            migrationBuilder.DropColumn(
                name: "MinFunding",
                table: "Investors");
        }
    }
}
