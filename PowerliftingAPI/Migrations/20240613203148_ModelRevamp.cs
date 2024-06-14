using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModelRevamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                table: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "ExerciseLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutExercises",
                table: "WorkoutExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "IsCustom",
                table: "CustomExercises");

            migrationBuilder.RenameTable(
                name: "WorkoutExercises",
                newName: "ExercisesInWorkout");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "ExercisesInWorkout",
                newName: "Repetitions");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutExercises_WorkoutId",
                table: "ExercisesInWorkout",
                newName: "IX_ExercisesInWorkout_WorkoutId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Workouts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CustomExercisesId",
                table: "ExercisesInWorkout",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesId",
                table: "ExercisesInWorkout",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "ExercisesInWorkout",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExercisesInWorkout",
                table: "ExercisesInWorkout",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesInWorkout_CustomExercisesId",
                table: "ExercisesInWorkout",
                column: "CustomExercisesId");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesInWorkout_ExercisesId",
                table: "ExercisesInWorkout",
                column: "ExercisesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesInWorkout_CustomExercises_CustomExercisesId",
                table: "ExercisesInWorkout",
                column: "CustomExercisesId",
                principalTable: "CustomExercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesInWorkout_Exercises_ExercisesId",
                table: "ExercisesInWorkout",
                column: "ExercisesId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesInWorkout_Workouts_WorkoutId",
                table: "ExercisesInWorkout",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesInWorkout_CustomExercises_CustomExercisesId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesInWorkout_Exercises_ExercisesId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesInWorkout_Workouts_WorkoutId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExercisesInWorkout",
                table: "ExercisesInWorkout");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesInWorkout_CustomExercisesId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesInWorkout_ExercisesId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "CustomExercisesId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropColumn(
                name: "ExercisesId",
                table: "ExercisesInWorkout");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ExercisesInWorkout");

            migrationBuilder.RenameTable(
                name: "ExercisesInWorkout",
                newName: "WorkoutExercises");

            migrationBuilder.RenameColumn(
                name: "Repetitions",
                table: "WorkoutExercises",
                newName: "ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExercisesInWorkout_WorkoutId",
                table: "WorkoutExercises",
                newName: "IX_WorkoutExercises_WorkoutId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustom",
                table: "CustomExercises",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutExercises",
                table: "WorkoutExercises",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ExerciseLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutExerciseId = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<int>(type: "int", nullable: false),
                    SetNumber = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseLogs_WorkoutExercises_WorkoutExerciseId",
                        column: x => x.WorkoutExerciseId,
                        principalTable: "WorkoutExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CustomExercises",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsCustom",
                value: false);

            migrationBuilder.UpdateData(
                table: "CustomExercises",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsCustom",
                value: true);

            migrationBuilder.UpdateData(
                table: "CustomExercises",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsCustom",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutExerciseId",
                table: "ExerciseLogs",
                column: "WorkoutExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalTable: "CustomExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Workouts_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
