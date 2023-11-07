using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HunterX.Trader.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerSymbols");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Symbol = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderSide = table.Column<string>(type: "varchar", nullable: false),
                    AssetClass = table.Column<string>(type: "varchar", nullable: false),
                    OrderType = table.Column<string>(type: "varchar", nullable: false),
                    TimeInForce = table.Column<string>(type: "varchar", nullable: false),
                    StopOrderPrice = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    StopLossPrice = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    LimitPrice = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    TrailPrice = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    TrailPercent = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    OrderedPrice = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: false),
                    FilledPrice = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FilledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Orders_ParentOrderId",
                        column: x => x.ParentOrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "StockSymbols",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Market = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockSymbols", x => new { x.Symbol, x.Exchange });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ParentOrderId",
                table: "Orders",
                column: "ParentOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "StockSymbols");

            migrationBuilder.CreateTable(
                name: "TickerSymbols",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Market = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerSymbols", x => new { x.Ticker, x.Exchange });
                });
        }
    }
}
