using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankTwoAPI_Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    BankFullName = table.Column<string>(nullable: true),
                    BankShortName = table.Column<string>(nullable: true),
                    BankIdentificationCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPassports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Passport = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPassports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    VerificationCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    MobileNetworkName = table.Column<string>(nullable: true),
                    NetworkProviderRegistratonCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileNetworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    OtherNames = table.Column<string>(nullable: true),
                    BVNNumber = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    AccountType = table.Column<int>(nullable: false),
                    AccountBalance = table.Column<decimal>(nullable: false),
                    TransactionPin = table.Column<int>(nullable: false),
                    CustomerPassportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAccounts_CustomerPassports_CustomerPassportId",
                        column: x => x.CustomerPassportId,
                        principalTable: "CustomerPassports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountCustomerCategories",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCustomerCategories", x => new { x.CustomerAccountId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_AccountCustomerCategories_CustomerAccounts_CustomerAccountId",
                        column: x => x.CustomerAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCustomerCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirTimeTopUp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerAccountId = table.Column<int>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: true),
                    MobileNetworksId = table.Column<int>(nullable: false),
                    MobileNumber = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirTimeTopUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirTimeTopUp_CustomerAccounts_CustomerAccountId",
                        column: x => x.CustomerAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirTimeTopUp_MobileNetworks_MobileNetworksId",
                        column: x => x.MobileNetworksId,
                        principalTable: "MobileNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerAccountId = table.Column<int>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CardType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardRequests_CustomerAccounts_CustomerAccountId",
                        column: x => x.CustomerAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBeneficiaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true),
                    BankId = table.Column<int>(nullable: false),
                    RecipientAccountNumber = table.Column<string>(nullable: true),
                    RecipientAccountName = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    OTPCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBeneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerBeneficiaries_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerBeneficiaries_CustomerAccounts_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterBankTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerAccountId = table.Column<int>(nullable: false),
                    BanksId = table.Column<int>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true),
                    TransferAmount = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RecipientAccountName = table.Column<string>(nullable: true),
                    RecipientAccountNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterBankTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterBankTransfers_Banks_BanksId",
                        column: x => x.BanksId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterBankTransfers_CustomerAccounts_CustomerAccountId",
                        column: x => x.CustomerAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntraBankTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerAccountId = table.Column<int>(nullable: false),
                    CustomerAccountNumber = table.Column<string>(nullable: true),
                    TransferAmount = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RecipientAccountName = table.Column<string>(nullable: true),
                    RecipientAccountNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntraBankTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntraBankTransfers_CustomerAccounts_CustomerAccountId",
                        column: x => x.CustomerAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCustomerCategories_CustomerId",
                table: "AccountCustomerCategories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AirTimeTopUp_CustomerAccountId",
                table: "AirTimeTopUp",
                column: "CustomerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AirTimeTopUp_MobileNetworksId",
                table: "AirTimeTopUp",
                column: "MobileNetworksId");

            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_CustomerAccountId",
                table: "CardRequests",
                column: "CustomerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_CustomerPassportId",
                table: "CustomerAccounts",
                column: "CustomerPassportId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBeneficiaries_BankId",
                table: "CustomerBeneficiaries",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBeneficiaries_CustomerId",
                table: "CustomerBeneficiaries",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InterBankTransfers_BanksId",
                table: "InterBankTransfers",
                column: "BanksId");

            migrationBuilder.CreateIndex(
                name: "IX_InterBankTransfers_CustomerAccountId",
                table: "InterBankTransfers",
                column: "CustomerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_IntraBankTransfers_CustomerAccountId",
                table: "IntraBankTransfers",
                column: "CustomerAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCustomerCategories");

            migrationBuilder.DropTable(
                name: "AirTimeTopUp");

            migrationBuilder.DropTable(
                name: "CardRequests");

            migrationBuilder.DropTable(
                name: "CustomerBeneficiaries");

            migrationBuilder.DropTable(
                name: "InterBankTransfers");

            migrationBuilder.DropTable(
                name: "IntraBankTransfers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "MobileNetworks");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "CustomerAccounts");

            migrationBuilder.DropTable(
                name: "CustomerPassports");
        }
    }
}
