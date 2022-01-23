using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Password_Manager.Infrastructure.Migrations
{
    public partial class salt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Password",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Password");
        }
    }
}
