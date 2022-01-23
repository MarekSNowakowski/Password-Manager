using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Password_Manager.Infrastructure.Migrations
{
    public partial class passEncrypted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pass",
                table: "Password");

            migrationBuilder.AddColumn<byte[]>(
                name: "PassEncrypted",
                table: "Password",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassEncrypted",
                table: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Pass",
                table: "Password",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
