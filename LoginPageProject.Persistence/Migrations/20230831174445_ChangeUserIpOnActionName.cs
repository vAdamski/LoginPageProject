using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginPageProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIpOnActionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserIp",
                table: "UserActionInfos",
                newName: "Action");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "UserActionInfos",
                newName: "UserIp");
        }
    }
}
