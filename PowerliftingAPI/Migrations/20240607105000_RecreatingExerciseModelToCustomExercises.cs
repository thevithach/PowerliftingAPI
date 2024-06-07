using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class RecreatingExerciseModelToCustomExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_UserId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "CustomExercises");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_UserId",
                table: "CustomExercises",
                newName: "IX_CustomExercises_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomExercises",
                table: "CustomExercises",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomExercises_AspNetUsers_UserId",
                table: "CustomExercises",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalTable: "CustomExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomExercises_AspNetUsers_UserId",
                table: "CustomExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomExercises",
                table: "CustomExercises");

            migrationBuilder.RenameTable(
                name: "CustomExercises",
                newName: "Exercises");

            migrationBuilder.RenameIndex(
                name: "IX_CustomExercises_UserId",
                table: "Exercises",
                newName: "IX_Exercises_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_UserId",
                table: "Exercises",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
