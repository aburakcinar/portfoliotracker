using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStockItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockItems",
                columns: table => new
                {
                    FullCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StockExchangeCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LocaleCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    WebSite = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItems", x => x.FullCode);
                    table.ForeignKey(
                        name: "FK_StockItems_Exchanges_StockExchangeCode",
                        column: x => x.StockExchangeCode,
                        principalTable: "Exchanges",
                        principalColumn: "Mic",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockItems_Locales_LocaleCode",
                        column: x => x.LocaleCode,
                        principalTable: "Locales",
                        principalColumn: "LocaleCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_LocaleCode",
                table: "StockItems",
                column: "LocaleCode");

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_StockExchangeCode",
                table: "StockItems",
                column: "StockExchangeCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockItems");
        }
    }
}
