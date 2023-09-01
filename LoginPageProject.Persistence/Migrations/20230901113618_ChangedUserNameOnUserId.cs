using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginPageProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserNameOnUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserName",
                table: "OldUserPasswords");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "OldUserPasswords",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OldUserPasswords_UserName",
                table: "OldUserPasswords",
                newName: "IX_OldUserPasswords_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserId",
                table: "OldUserPasswords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserId",
                table: "OldUserPasswords");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OldUserPasswords",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_OldUserPasswords_UserId",
                table: "OldUserPasswords",
                newName: "IX_OldUserPasswords_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserName",
                table: "OldUserPasswords",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
