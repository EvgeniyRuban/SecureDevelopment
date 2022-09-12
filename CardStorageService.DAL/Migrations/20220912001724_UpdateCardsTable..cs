using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardStorageService.DAL.Migrations
{
    public partial class UpdateCardsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HolderName",
                table: "Cards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HolderName",
                table: "Cards",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
