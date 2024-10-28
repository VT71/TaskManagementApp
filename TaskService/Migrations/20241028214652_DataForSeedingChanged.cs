using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Migrations
{
    /// <inheritdoc />
    public partial class DataForSeedingChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Completed",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Completed",
                value: false);
        }
    }
}
