using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class createAiBusinessStandardsEvaluationsTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiBusinessStandardsEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    CategoryStandardId = table.Column<int>(type: "int", nullable: false),
                    AchievementScore = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: true),
                    WeightedContribution = table.Column<int>(type: "int", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiBusinessStandardsEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiBusinessStandardsEvaluations_Business_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Business",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AiBusinessStandardsEvaluations_CategoryStandard_CategoryStandardId",
                        column: x => x.CategoryStandardId,
                        principalTable: "CategoryStandard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiBusinessStandardsEvaluations_BusinessId",
                table: "AiBusinessStandardsEvaluations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_AiBusinessStandardsEvaluations_CategoryStandardId",
                table: "AiBusinessStandardsEvaluations",
                column: "CategoryStandardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiBusinessStandardsEvaluations");
        }
    }
}
