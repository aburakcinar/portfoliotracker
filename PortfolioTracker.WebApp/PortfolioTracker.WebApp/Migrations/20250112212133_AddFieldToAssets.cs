using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "Assets",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PortfolioV2s",
                keyColumn: "Id",
                keyValue: new Guid("25d4cfe9-076d-48e1-a04b-a0b139bb8864"),
                column: "Created",
                value: new DateTime(2025, 1, 12, 21, 21, 33, 203, DateTimeKind.Utc).AddTicks(9464));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "Assets");

            migrationBuilder.UpdateData(
                table: "PortfolioV2s",
                keyColumn: "Id",
                keyValue: new Guid("25d4cfe9-076d-48e1-a04b-a0b139bb8864"),
                column: "Created",
                value: new DateTime(2025, 1, 12, 17, 47, 16, 707, DateTimeKind.Utc).AddTicks(3024));
        }
    }
}
