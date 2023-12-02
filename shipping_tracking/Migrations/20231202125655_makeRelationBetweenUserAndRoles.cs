using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipping_tracking.Migrations
{
    /// <inheritdoc />
    public partial class makeRelationBetweenUserAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UserInfoId",
                table: "AspNetRoles",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_Users_UserInfoId",
                table: "AspNetRoles",
                column: "UserInfoId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_Users_UserInfoId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_UserInfoId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "AspNetRoles");
        }
    }
}
