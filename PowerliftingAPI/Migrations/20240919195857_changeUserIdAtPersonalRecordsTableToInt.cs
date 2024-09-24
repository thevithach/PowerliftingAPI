using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerliftingAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeUserIdAtPersonalRecordsTableToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalRecords_AspNetUsers_UserId1",
                table: "PersonalRecords");

            migrationBuilder.DropIndex(
                name: "IX_PersonalRecords_UserId1",
                table: "PersonalRecords");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PersonalRecords");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PersonalRecords",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRecords_UserId",
                table: "PersonalRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalRecords_AspNetUsers_UserId",
                table: "PersonalRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalRecords_AspNetUsers_UserId",
                table: "PersonalRecords");

            migrationBuilder.DropIndex(
                name: "IX_PersonalRecords_UserId",
                table: "PersonalRecords");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PersonalRecords",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "PersonalRecords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRecords_UserId1",
                table: "PersonalRecords",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalRecords_AspNetUsers_UserId1",
                table: "PersonalRecords",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
