using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginPageProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLoginAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedTo",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LoginAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedTo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LoginAttempts",
                table: "AspNetUsers");
        }
    }
}
