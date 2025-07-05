using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investly.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeAndSubjectToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserTypeFrom",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "UserTypeTo",
                table: "Feedback",
                newName: "FeedbackType");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Feedback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "FeedbackType",
                table: "Feedback",
                newName: "UserTypeTo");

            migrationBuilder.AddColumn<int>(
                name: "UserTypeFrom",
                table: "Feedback",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
