using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "EUR");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "TRY");

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "USD");

            migrationBuilder.AddColumn<string>(
                name: "NameLocal",
                table: "Currencies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubunitName",
                table: "Currencies",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubunitValue",
                table: "Currencies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PortfolioV2s",
                keyColumn: "Id",
                keyValue: new Guid("25d4cfe9-076d-48e1-a04b-a0b139bb8864"),
                column: "Created",
                value: new DateTime(2025, 1, 18, 21, 42, 11, 992, DateTimeKind.Utc).AddTicks(6862));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameLocal",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "SubunitName",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "SubunitValue",
                table: "Currencies");

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Code", "Name", "Symbol" },
                values: new object[,]
                {
                    { "EUR", "Euro", "€" },
                    { "TRY", "Turk Lirasi", "₺" },
                    { "USD", "United States Dollar", "$" }
                });

            migrationBuilder.UpdateData(
                table: "PortfolioV2s",
                keyColumn: "Id",
                keyValue: new Guid("25d4cfe9-076d-48e1-a04b-a0b139bb8864"),
                column: "Created",
                value: new DateTime(2025, 1, 12, 21, 21, 33, 203, DateTimeKind.Utc).AddTicks(9464));
        }
    }
}
