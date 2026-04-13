using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pd.Tasks.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HoursWorked = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "Description", "DueDate", "HoursWorked", "IsActive", "Status", "Title" },
                values: new object[,]
                {
                    { 11, null, new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Users reporting issues with password reset flow", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Utc), 0, true, 1, "Fix authentication bug" },
                    { 13, new DateTime(2026, 4, 11, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 12, 0, 0, 0, DateTimeKind.Utc), "Cover task service with xUnit tests", new DateTime(2026, 4, 16, 12, 0, 0, 0, DateTimeKind.Utc), 6, false, 3, "Write unit tests" },
                    { 14, null, new DateTime(2026, 4, 11, 14, 0, 0, 0, DateTimeKind.Utc), "Create responsive login form for Prodash", new DateTime(2026, 4, 20, 12, 0, 0, 0, DateTimeKind.Utc), 0, true, 1, "Design login UI" },
                    { 15, null, new DateTime(2026, 4, 8, 12, 0, 0, 0, DateTimeKind.Utc), "Reset EF Core migrations for fresh start", new DateTime(2026, 4, 15, 12, 0, 0, 0, DateTimeKind.Utc), 2, true, 2, "Database migration cleanup" },
                    { 16, null, new DateTime(2026, 4, 7, 12, 0, 0, 0, DateTimeKind.Utc), "Implement filtering by status in dashboard", new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), 0, true, 1, "Add task filtering" },
                    { 17, null, new DateTime(2026, 4, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Configure GitHub Actions for Prodash", new DateTime(2026, 4, 13, 12, 0, 0, 0, DateTimeKind.Utc), 4, true, 2, "Setup CI/CD pipeline" },
                    { 18, new DateTime(2026, 4, 11, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Document endpoints for task management", new DateTime(2026, 4, 12, 12, 0, 0, 0, DateTimeKind.Utc), 5, false, 3, "Write API documentation" },
                    { 19, null, new DateTime(2026, 4, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Implement dark theme toggle in UI", new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), 0, true, 1, "Add dark mode" },
                    { 20, null, new DateTime(2026, 4, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Improve EF Core query performance", new DateTime(2026, 4, 10, 12, 0, 0, 0, DateTimeKind.Utc), 2, true, 2, "Optimize queries" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
