using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class removeRepetitionsAndWeightFromWorkoutExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repetitions",
                table: "ExercisesInWorkout");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ExercisesInWorkout");

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutExerciseId = table.Column<int>(type: "int", nullable: false),
                    SetNumber = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_ExercisesInWorkout_WorkoutExerciseId",
                        column: x => x.WorkoutExerciseId,
                        principalTable: "ExercisesInWorkout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutExerciseId",
                table: "Sets",
                column: "WorkoutExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.AddColumn<int>(
                name: "Repetitions",
                table: "ExercisesInWorkout",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "ExercisesInWorkout",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
