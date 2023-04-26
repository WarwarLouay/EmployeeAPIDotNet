using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class hashPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "user");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "user",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "user",
                type: "longblob",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "user");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
