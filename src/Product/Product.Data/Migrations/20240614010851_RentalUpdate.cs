using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Data.Migrations
{
    /// <inheritdoc />
    public partial class RentalUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalExtras",
                table: "Rental",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalFines",
                table: "Rental",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalRental",
                table: "Rental",
                type: "numeric(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalExtras",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "TotalFines",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "TotalRental",
                table: "Rental");
        }
    }
}
