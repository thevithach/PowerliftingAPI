using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToWorkoutModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Workouts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Workouts");
        }
    }
}
