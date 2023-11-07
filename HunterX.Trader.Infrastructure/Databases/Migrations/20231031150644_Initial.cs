using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HunterX.Trader.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketHolidays",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Open = table.Column<TimeSpan>(type: "time", nullable: true),
                    Close = table.Column<TimeSpan>(type: "time", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketHolidays", x => new { x.Date, x.Exchange });
                });

            migrationBuilder.CreateTable(
                name: "TickerSymbols",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Market = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerSymbols", x => new { x.Ticker, x.Exchange });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketHolidays");

            migrationBuilder.DropTable(
                name: "TickerSymbols");
        }
    }
}
