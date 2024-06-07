using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModelRevampRemovedExerciseLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "IsCustom",
                table: "CustomExercises");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Workouts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseId",
                table: "WorkoutExercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomExerciseId",
                table: "WorkoutExercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomExercisesId",
                table: "WorkoutExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesId",
                table: "WorkoutExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Repetitions",
                table: "WorkoutExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "WorkoutExercises",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                name: "IX_WorkoutExercises_CustomExercisesId",
                table: "WorkoutExercises",
                column: "CustomExercisesId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExercisesId",
                table: "WorkoutExercises",
                column: "ExercisesId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_CustomExercisesId",
                table: "WorkoutExercises",
                column: "CustomExercisesId",
                principalTable: "CustomExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExercisesId",
                table: "WorkoutExercises",
                column: "ExercisesId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_CustomExercises_CustomExercisesId",
                table: "WorkoutExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExercisesId",
                table: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutExercises_CustomExercisesId",
                table: "WorkoutExercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutExercises_ExercisesId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "CustomExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "CustomExercisesId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "ExercisesId",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "Repetitions",
                table: "WorkoutExercises");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "WorkoutExercises");

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseId",
                table: "WorkoutExercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustom",
                table: "CustomExercises",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }
    }
}
