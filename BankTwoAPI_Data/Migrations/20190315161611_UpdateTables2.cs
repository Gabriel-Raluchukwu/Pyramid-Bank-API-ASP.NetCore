using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTwoAPI_Data.Migrations
{
    public partial class UpdateTables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                table: "CustomerAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerPassportId",
                table: "CustomerAccounts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                table: "CustomerAccounts",
                column: "CustomerPassportId",
                principalTable: "CustomerPassports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                table: "CustomerAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerPassportId",
                table: "CustomerAccounts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                table: "CustomerAccounts",
                column: "CustomerPassportId",
                principalTable: "CustomerPassports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
