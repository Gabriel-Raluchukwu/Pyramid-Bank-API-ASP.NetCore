using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTwoAPI_Data.Migrations
{
    public partial class UpdateTable4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "AirTimeTopUp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "AirTimeTopUp",
                nullable: true);
        }
    }
}
