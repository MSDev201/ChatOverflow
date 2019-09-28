using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatOverflow.Migrations
{
    public partial class ChangedSomeGroupChatFieldsWithAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordAccess",
                table: "GroupChats");

            migrationBuilder.DropColumn(
                name: "UsersInList",
                table: "GroupChats");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "GroupChats",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "GroupChats");

            migrationBuilder.AddColumn<bool>(
                name: "PasswordAccess",
                table: "GroupChats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsersInList",
                table: "GroupChats",
                nullable: false,
                defaultValue: false);
        }
    }
}
