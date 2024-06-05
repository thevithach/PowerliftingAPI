using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class TestDataForExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Description", "IsCustom", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Description for Exercise 1", false, "Exercise 1", null },
                    { 2, "Description for Exercise 2", true, "Exercise 2", null },
                    { 3, "Description for Exercise 3", false, "Exercise 3", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
