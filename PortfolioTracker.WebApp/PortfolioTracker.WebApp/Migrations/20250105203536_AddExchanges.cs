using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddExchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Mic = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OperatingMic = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    OprtSgmt = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    MarketNameInstitutionDescription = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LegalEntityName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Lei = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MarketCategoryCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Acronym = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    City = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    WebSite = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastValidationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Comments = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Mic);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exchanges");
        }
    }
}
