using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginPageProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdToUserActionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserId",
                table: "OldUserPasswords");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "UserActionInfos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserActionInfos_UserName",
                table: "UserActionInfos",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserId",
                table: "OldUserPasswords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActionInfos_AspNetUsers_UserName",
                table: "UserActionInfos",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserId",
                table: "OldUserPasswords");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActionInfos_AspNetUsers_UserName",
                table: "UserActionInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserActionInfos_UserName",
                table: "UserActionInfos");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "UserActionInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_OldUserPasswords_AspNetUsers_UserId",
                table: "OldUserPasswords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
