using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Holdings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "BankAccountTransactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BankAccountTransactionGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExecuteDate",
                table: "BankAccountTransactionGroups",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "BankAccountTransactionGroups",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Holdings");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "BankAccountTransactions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BankAccountTransactionGroups");

            migrationBuilder.DropColumn(
                name: "ExecuteDate",
                table: "BankAccountTransactionGroups");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "BankAccountTransactionGroups");
        }
    }
}
