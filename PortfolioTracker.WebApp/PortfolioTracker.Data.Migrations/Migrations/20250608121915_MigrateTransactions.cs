using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class MigrateTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountTransactions_TransactionActionTypes_ActionTypeCo~",
                table: "BankAccountTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankAccountTransactions_ActionTypeCode",
                table: "BankAccountTransactions");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "TransactionActionTypes");

            migrationBuilder.DropColumn(
                name: "ActionTypeCode",
                table: "BankAccountTransactions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "BankAccountTransactions");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "BankAccountTransactions");

            migrationBuilder.DropColumn(
                name: "State",
                table: "BankAccountTransactionGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BankAccountTransactions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "BankAccountTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExecuteDate",
                table: "BankAccountTransactionGroups",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "ActionTypeCode",
                table: "BankAccountTransactionGroups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountTransactionGroups_ActionTypeCode",
                table: "BankAccountTransactionGroups",
                column: "ActionTypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountTransactionGroups_TransactionActionTypes_ActionT~",
                table: "BankAccountTransactionGroups",
                column: "ActionTypeCode",
                principalTable: "TransactionActionTypes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountTransactionGroups_TransactionActionTypes_ActionT~",
                table: "BankAccountTransactionGroups");

            migrationBuilder.DropIndex(
                name: "IX_BankAccountTransactionGroups_ActionTypeCode",
                table: "BankAccountTransactionGroups");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "BankAccountTransactions");

            migrationBuilder.DropColumn(
                name: "ActionTypeCode",
                table: "BankAccountTransactionGroups");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "TransactionActionTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BankAccountTransactions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionTypeCode",
                table: "BankAccountTransactions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "BankAccountTransactions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "BankAccountTransactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ExecuteDate",
                table: "BankAccountTransactionGroups",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "BankAccountTransactionGroups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountTransactions_ActionTypeCode",
                table: "BankAccountTransactions",
                column: "ActionTypeCode");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountTransactions_TransactionActionTypes_ActionTypeCo~",
                table: "BankAccountTransactions",
                column: "ActionTypeCode",
                principalTable: "TransactionActionTypes",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
