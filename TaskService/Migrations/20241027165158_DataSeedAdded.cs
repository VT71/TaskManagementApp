using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskService.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "Id", "Completed", "Description", "DueDate", "Title" },
                values: new object[,]
                {
                    { 1L, false, null, new DateTime(2025, 10, 27, 15, 23, 59, 689, DateTimeKind.Local), "Task 1" },
                    { 2L, false, null, new DateTime(2025, 10, 27, 15, 23, 59, 689, DateTimeKind.Local), "Task 2" },
                    { 3L, false, null, new DateTime(2025, 10, 27, 15, 23, 59, 689, DateTimeKind.Local), "Task 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
