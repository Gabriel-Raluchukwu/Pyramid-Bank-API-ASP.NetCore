using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTwoAPI_Data.Migrations
{
    public partial class UpdateTables3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                table: "CustomerAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAccounts_CustomerPassportId",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "CustomerPassportId",
                table: "CustomerAccounts");

            migrationBuilder.AddColumn<int>(
                name: "CustomerAccountId",
                table: "CustomerPassports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPassports_CustomerAccountId",
                table: "CustomerPassports",
                column: "CustomerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPassports_CustomerAccounts_CustomerAccountId",
                table: "CustomerPassports",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPassports_CustomerAccounts_CustomerAccountId",
                table: "CustomerPassports");

            migrationBuilder.DropIndex(
                name: "IX_CustomerPassports_CustomerAccountId",
                table: "CustomerPassports");

            migrationBuilder.DropColumn(
                name: "CustomerAccountId",
                table: "CustomerPassports");

            migrationBuilder.AddColumn<int>(
                name: "CustomerPassportId",
                table: "CustomerAccounts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_CustomerPassportId",
                table: "CustomerAccounts",
                column: "CustomerPassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                table: "CustomerAccounts",
                column: "CustomerPassportId",
                principalTable: "CustomerPassports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
