using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTwoAPI_Data.Migrations
{
    public partial class SeedBankAndNetworkData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "BankFullName", "BankIdentificationCode", "BankShortName", "CreatedAt", "IsActive", "LastUpdatedAt" },
                values: new object[,]
                {
                    { 1, "Access Bank", 86428357, "Access Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "Wema Bank Plc", 88387850, "Wema Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "Keystone Bank Limited", 32404055, "Keystone Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Heritage Banking Company Limited", 15094290, "Heritage Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "First City Monument Bank Plc", 48299192, "First City Monument Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Sterling Bank Plc", 65002961, "Sterling Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Stanbic IBTC Bank Plc", 58811713, "Stanbic IBTC Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Standard Chartered", 88427198, "Standard Chatered Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Ecobank Nigeria Plc", 49546921, "Eco Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Zenith Bank Plc", 60412621, "Zenith Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Union Bank for Africa Plc", 42359561, "Union Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Guaranty Trust Bank Plc", 12039523, "Guaranty Trust Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "First Bank of Nigeria Limited", 49968453, "First Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Fidelity Bank", 15090085, "Fidelity Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Diamond Bank Plc", 75473128, "Diamond Bank", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "MobileNetworks",
                columns: new[] { "Id", "CreatedAt", "IsActive", "LastUpdatedAt", "MobileNetworkName", "NetworkProviderRegistratonCode" },
                values: new object[,]
                {
                    { 3, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AIRTEL", 123456789 },
                    { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MTN", 123456789 },
                    { 2, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GLO", 123456789 },
                    { 4, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ETISALAT", 123456789 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MobileNetworks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MobileNetworks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MobileNetworks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MobileNetworks",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
