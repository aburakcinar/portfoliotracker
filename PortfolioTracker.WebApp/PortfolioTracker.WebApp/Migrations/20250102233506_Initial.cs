using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StockExchange = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InOut = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TransactionGroupId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionGroups_TransactionGroupId",
                        column: x => x.TransactionGroupId,
                        principalTable: "TransactionGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Holdings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PortfolioId = table.Column<Guid>(type: "uuid", nullable: false),
                    StockId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holdings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holdings_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Holdings_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Holdings_TransactionGroups_TransactionGroupId",
                        column: x => x.TransactionGroupId,
                        principalTable: "TransactionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Code", "Name", "Symbol" },
                values: new object[,]
                {
                    { "EUR", "Euro", "€" },
                    { "TRY", "Turk Lirasi", "₺" },
                    { "USD", "United States Dollar", "$" }
                });

            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Id", "Description", "Name", "StockExchange", "Symbol" },
                values: new object[,]
                {
                    { new Guid("1cc7e8b4-f4f0-4cf5-bdb7-a9947f98c55a"), "", "Doğuş Otomotiv", "XIST", "DOAS" },
                    { new Guid("44829b63-9b2c-4153-ad4d-b194aa5625b6"), "", "Ereğli Demir ve Çelik Fabrikaları", "XIST", "EREGL" },
                    { new Guid("4a4ee229-7d3e-4490-850b-3f467862ff15"), "", "Baskent Dogalgaz Dagitim Gayr Yat OrtAS", "XIST", "BASGZ" },
                    { new Guid("5163feb7-5ceb-4d65-8bfb-64bcad4f370e"), "", "Ford Otosan", "XIST", "FROTO" },
                    { new Guid("64eaa2bd-80ab-46a8-9eb3-3112ac1e73b1"), "", "İş Yatırım Menkul Değerler", "XIST", "ISMEN" },
                    { new Guid("7a46d600-2cc6-4e45-ab74-876064839746"), "", "Türk Traktör ve Ziraat Makineleri", "XIST", "TTRAK" },
                    { new Guid("7f28b900-27eb-4f4a-856a-1e1618ee59ac"), "", "Türk Hava Yolları", "XIST", "THYAO" },
                    { new Guid("a242dc19-adc4-4c8f-a827-416fd8a391b4"), "", "Enerjisa", "XIST", "ENJSA" },
                    { new Guid("b2aa0d1f-f511-4940-a118-38dc6b93b5d1"), "", "Vestel Beyaz Eşya", "XIST", "VESBE" },
                    { new Guid("b37a0f69-d398-4cec-a460-2835a06ba6bc"), "", "Türkiye Garanti Bankası", "XIST", "GARAN" },
                    { new Guid("ee0111bd-bb3e-42f8-9e31-42cf06d71efa"), "", "Indeks Blgsyr Sstmlr Mhndslk Sny v Tcrt", "XIST", "INDES" },
                    { new Guid("f7c5e6c8-78a5-4325-ba93-e5f451483866"), "", "Tekfen Holding", "XIST", "TKFEN" },
                    { new Guid("f99512b6-324d-4e13-b75a-19e9301f1565"), "", "TÜPRAŞ-Türkiye Petrol Rafinerileri", "XIST", "TUPRS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_PortfolioId",
                table: "Holdings",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_StockId",
                table: "Holdings",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Holdings_TransactionGroupId",
                table: "Holdings",
                column: "TransactionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_CurrencyCode",
                table: "Portfolios",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionGroupId",
                table: "Transactions",
                column: "TransactionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holdings");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "TransactionGroups");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
