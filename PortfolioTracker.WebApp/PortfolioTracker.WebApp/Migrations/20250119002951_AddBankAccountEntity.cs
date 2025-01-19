using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBankAccountEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    BankName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AccountHolder = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Iban = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    LocaleCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    OpenDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Locales_LocaleCode",
                        column: x => x.LocaleCode,
                        principalTable: "Locales",
                        principalColumn: "LocaleCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionActionTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionActionTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountTransactionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountTransactionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountTransactionGroups_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountTransactionGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InOut = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountTransactions_BankAccountTransactionGroups_BankAc~",
                        column: x => x.BankAccountTransactionGroupId,
                        principalTable: "BankAccountTransactionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PortfolioV2s",
                keyColumn: "Id",
                keyValue: new Guid("25d4cfe9-076d-48e1-a04b-a0b139bb8864"),
                column: "Created",
                value: new DateTime(2025, 1, 19, 0, 29, 51, 481, DateTimeKind.Utc).AddTicks(2456));

            migrationBuilder.InsertData(
                table: "TransactionActionTypes",
                columns: new[] { "Code", "Category", "Description" },
                values: new object[,]
                {
                    { "ACCOUNT_FEE", 1, "Account usage fee on target account" },
                    { "DEPOSIT", 1, "Deposit to target account" },
                    { "DEPOSIT_FEE", 3, "Deposit fee on target account" },
                    { "DEPOSIT_TAX", 2, "Tax on Deposit from target account" },
                    { "DIVIDEND_DISTRIBUTION", 1, "Dividend payment to target account" },
                    { "DIVIDEND_DISTRIBUTION_FEE", 3, "Dividend distribution fee on target account" },
                    { "DIVIDEND_WITHHOLDING_TAX", 2, "Dividend Withholding tax" },
                    { "INTEREST", 1, "Interest payment to target account" },
                    { "INTEREST_FEE", 3, "Interest fee from target account" },
                    { "INTEREST_TAX", 2, "Tax on Interest from target account" },
                    { "PAYMENT", 1, "Payment from target account" },
                    { "PAYMENT_FEE", 3, "Payment action fee on target account" },
                    { "PAYMENT_TAX", 2, "Tax on Payment action on target account" },
                    { "WITHDRAW", 1, "Withdraw from target account" },
                    { "WITHDRAW_FEE", 3, "Withdraw action fee on target account" },
                    { "WITHDRAW_TAX", 2, "Tax on Withdraw action on target account" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CurrencyCode",
                table: "BankAccounts",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_LocaleCode",
                table: "BankAccounts",
                column: "LocaleCode");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountTransactionGroups_BankAccountId",
                table: "BankAccountTransactionGroups",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountTransactions_BankAccountTransactionGroupId",
                table: "BankAccountTransactions",
                column: "BankAccountTransactionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccountTransactions");

            migrationBuilder.DropTable(
                name: "TransactionActionTypes");

            migrationBuilder.DropTable(
                name: "BankAccountTransactionGroups");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.UpdateData(
                table: "PortfolioV2s",
                keyColumn: "Id",
                keyValue: new Guid("25d4cfe9-076d-48e1-a04b-a0b139bb8864"),
                column: "Created",
                value: new DateTime(2025, 1, 18, 21, 42, 11, 992, DateTimeKind.Utc).AddTicks(6862));
        }
    }
}
