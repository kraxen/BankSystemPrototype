using Microsoft.EntityFrameworkCore.Migrations;

namespace BankSystemPrototype.ApplicationServices.Migrations
{
    public partial class Correct_Structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequrityKey",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "SequrityKey",
                table: "Banks");
        }
    }
}
