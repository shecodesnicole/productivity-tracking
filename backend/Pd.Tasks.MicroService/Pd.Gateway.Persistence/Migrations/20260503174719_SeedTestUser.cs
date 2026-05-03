using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pd.Tasks.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "IsActive", "Password", "PasswordResetToken", "PasswordResetTokenExpiry", "UserRole" },
                values: new object[] { "user-seed-1", "test@prodash.com", true, "cHJvZGFzaGRldnNhbHQxMqHLMDawVBMgIT8x3zJD4CuwfI8v", null, null, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "user-seed-1");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
