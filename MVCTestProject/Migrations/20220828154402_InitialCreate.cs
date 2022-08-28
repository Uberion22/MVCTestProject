using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCTestProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cryptocurrencies",
                columns: table => new
                {
                    CryptocurrencyServerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumMarketPairs = table.Column<int>(type: "int", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxSupply = table.Column<double>(type: "float", nullable: true),
                    CirculatingSupply = table.Column<double>(type: "float", nullable: true),
                    TotalSupply = table.Column<double>(type: "float", nullable: true),
                    CmcRank = table.Column<int>(type: "int", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptocurrencies", x => x.CryptocurrencyServerId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoMetadatas",
                columns: table => new
                {
                    CryptocurrencyServerId = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLaunched = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoMetadatas", x => x.CryptocurrencyServerId);
                    table.ForeignKey(
                        name: "FK_CryptoMetadatas_Cryptocurrencies_CryptocurrencyServerId",
                        column: x => x.CryptocurrencyServerId,
                        principalTable: "Cryptocurrencies",
                        principalColumn: "CryptocurrencyServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    CryprocurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.CryprocurrencyId);
                    table.ForeignKey(
                        name: "FK_Quotes_Cryptocurrencies_CryprocurrencyId",
                        column: x => x.CryprocurrencyId,
                        principalTable: "Cryptocurrencies",
                        principalColumn: "CryptocurrencyServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuoteItems",
                columns: table => new
                {
                    QuoteId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Volume24h = table.Column<double>(type: "float", nullable: true),
                    VolumeChange24h = table.Column<double>(type: "float", nullable: true),
                    PercentChange1h = table.Column<double>(type: "float", nullable: true),
                    PercentChange24h = table.Column<double>(type: "float", nullable: true),
                    PercentChange7d = table.Column<double>(type: "float", nullable: true),
                    MarketCap = table.Column<double>(type: "float", nullable: true),
                    MarketCapDominance = table.Column<double>(type: "float", nullable: true),
                    FullyDilutedMarketCap = table.Column<double>(type: "float", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteItems", x => x.QuoteId);
                    table.ForeignKey(
                        name: "FK_QuoteItems_Quotes_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Quotes",
                        principalColumn: "CryprocurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoMetadatas");

            migrationBuilder.DropTable(
                name: "QuoteItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "Cryptocurrencies");
        }
    }
}
