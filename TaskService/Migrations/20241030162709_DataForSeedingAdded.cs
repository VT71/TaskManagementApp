using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskService.Migrations
{
    /// <inheritdoc />
    public partial class DataForSeedingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoTasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Completed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoTasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "Id", "Completed", "Description", "DueDate", "Title" },
                values: new object[,]
                {
                    { 1L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut velit vel orci dapibus tristique.", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "Task A" },
                    { 2L, true, null, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Local), "Task B" },
                    { 3L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque non orci sit amet turpis cursus.", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Local), "Task C" },
                    { 4L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a massa nec elit vestibulum aliquam.", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Local), "Task D" },
                    { 5L, false, null, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Local), "Task E" },
                    { 6L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi volutpat tortor et magna bibendum.", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Local), "Task F" },
                    { 7L, false, null, new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Local), "Task G" },
                    { 8L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse a risus ac sapien ultricies.", new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Local), "Task H" },
                    { 9L, false, null, new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Local), "Task I" },
                    { 10L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vehicula nisl at elit feugiat.", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Local), "Task J" },
                    { 11L, false, null, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Local), "Task K" },
                    { 12L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur auctor orci sit amet mi.", new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Local), "Task L" },
                    { 13L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec tempor ligula eget cursus.", new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Local), "Task M" },
                    { 14L, true, null, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Local), "Task N" },
                    { 15L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus scelerisque orci vitae metus.", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Local), "Task O" },
                    { 16L, true, null, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Local), "Task P" },
                    { 17L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer at justo vel erat pharetra.", new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Local), "Task Q" },
                    { 18L, true, null, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Local), "Task R" },
                    { 19L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam et lacus pulvinar, porta eros.", new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Local), "Task S" },
                    { 20L, true, null, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Local), "Task T" },
                    { 21L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum sit amet lacus lacinia.", new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Local), "Task U" },
                    { 22L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non justo at orci commodo.", new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Local), "Task V" },
                    { 23L, false, null, new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Local), "Task W" },
                    { 24L, true, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consectetur orci id odio.", new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Local), "Task X" },
                    { 25L, false, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sed felis vestibulum, lacinia.", new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Local), "Task Y" },
                    { 26L, true, null, new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Local), "Task Z" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoTasks");
        }
    }
}
