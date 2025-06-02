using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class IncreasScaleOfAIRateAndCapital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Capital",
                table: "Business",
                type: "decimal(18,5)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AIRate",
                table: "Business",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Capital",
                table: "Business",
                type: "decimal(10,5)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AIRate",
                table: "Business",
                type: "decimal(3,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);
        }
    }
}
