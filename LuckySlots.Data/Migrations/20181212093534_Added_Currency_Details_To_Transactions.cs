using Microsoft.EntityFrameworkCore.Migrations;

namespace LuckySlots.Data.Migrations
{
    public partial class Added_Currency_Details_To_Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseCurrency",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseCurrencyAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "ExchangeRate",
                table: "Transactions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "QuotedCurrency",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuotedCurrencyAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseCurrency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BaseCurrencyAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "QuotedCurrency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "QuotedCurrencyAmount",
                table: "Transactions");
        }
    }
}
