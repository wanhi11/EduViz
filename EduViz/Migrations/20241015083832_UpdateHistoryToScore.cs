using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduViz.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHistoryToScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "subjectId",
                table: "Subjects",
                type: "NVARCHAR(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_studentQuizScoreId",
                table: "StudentAnswers",
                column: "studentQuizScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_StudentQuizScores_studentQuizScoreId",
                table: "StudentAnswers",
                column: "studentQuizScoreId",
                principalTable: "StudentQuizScores",
                principalColumn: "studentQuizScoreId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_StudentQuizScores_studentQuizScoreId",
                table: "StudentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswers_studentQuizScoreId",
                table: "StudentAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "subjectId",
                table: "Subjects",
                type: "NVARCHAR",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(36)");
        }
    }
}
