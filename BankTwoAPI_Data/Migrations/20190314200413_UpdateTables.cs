using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTwoAPI_Data.Migrations
{
    public partial class UpdateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTPCode",
                table: "CustomerBeneficiaries");

            migrationBuilder.AddColumn<int>(
                name: "RecipientBankId",
                table: "IntraBankTransfers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecipientBankName",
                table: "IntraBankTransfers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IntraBankTransfers_RecipientBankId",
                table: "IntraBankTransfers",
                column: "RecipientBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_IntraBankTransfers_Banks_RecipientBankId",
                table: "IntraBankTransfers",
                column: "RecipientBankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntraBankTransfers_Banks_RecipientBankId",
                table: "IntraBankTransfers");

            migrationBuilder.DropIndex(
                name: "IX_IntraBankTransfers_RecipientBankId",
                table: "IntraBankTransfers");

            migrationBuilder.DropColumn(
                name: "RecipientBankId",
                table: "IntraBankTransfers");

            migrationBuilder.DropColumn(
                name: "RecipientBankName",
                table: "IntraBankTransfers");

            migrationBuilder.AddColumn<int>(
                name: "OTPCode",
                table: "CustomerBeneficiaries",
                nullable: false,
                defaultValue: 0);
        }
    }
}
