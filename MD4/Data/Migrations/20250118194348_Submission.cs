using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MD4.Data.Migrations
{
    /// <inheritdoc />
    public partial class Submission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentFullName",
                table: "Submission");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_AssignmentId",
                table: "Submission",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_StudentId",
                table: "Submission",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Assignment_AssignmentId",
                table: "Submission",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_Students_StudentId",
                table: "Submission",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Assignment_AssignmentId",
                table: "Submission");

            migrationBuilder.DropForeignKey(
                name: "FK_Submission_Students_StudentId",
                table: "Submission");

            migrationBuilder.DropIndex(
                name: "IX_Submission_AssignmentId",
                table: "Submission");

            migrationBuilder.DropIndex(
                name: "IX_Submission_StudentId",
                table: "Submission");

            migrationBuilder.AddColumn<string>(
                name: "StudentFullName",
                table: "Submission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
