using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLocales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionGroups_TransactionGroupId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionGroupId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Locales",
                columns: table => new
                {
                    LocaleCode = table.Column<string>(type: "text", nullable: false),
                    LanguageName = table.Column<string>(type: "text", nullable: false),
                    LanguageNameLocal = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    CountryNameLocal = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: false),
                    CurrencyNameLocal = table.Column<string>(type: "text", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "text", nullable: false),
                    CurrencySubunitValue = table.Column<int>(type: "integer", nullable: false),
                    CurrencySubunitName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locales", x => x.LocaleCode);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionGroups_TransactionGroupId",
                table: "Transactions",
                column: "TransactionGroupId",
                principalTable: "TransactionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionGroups_TransactionGroupId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Locales");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransactionGroupId",
                table: "Transactions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionGroups_TransactionGroupId",
                table: "Transactions",
                column: "TransactionGroupId",
                principalTable: "TransactionGroups",
                principalColumn: "Id");
        }
    }
}
