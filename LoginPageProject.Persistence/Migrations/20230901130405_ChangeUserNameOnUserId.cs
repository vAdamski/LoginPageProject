using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginPageProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserNameOnUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActionInfos_AspNetUsers_UserName",
                table: "UserActionInfos");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserActionInfos",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserActionInfos_UserName",
                table: "UserActionInfos",
                newName: "IX_UserActionInfos_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActionInfos_AspNetUsers_UserId",
                table: "UserActionInfos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActionInfos_AspNetUsers_UserId",
                table: "UserActionInfos");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserActionInfos",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_UserActionInfos_UserId",
                table: "UserActionInfos",
                newName: "IX_UserActionInfos_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActionInfos_AspNetUsers_UserName",
                table: "UserActionInfos",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
