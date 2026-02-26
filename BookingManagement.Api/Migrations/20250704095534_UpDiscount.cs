using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Tickets",
                type: "numeric(12,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Tickets",
                type: "numeric(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromotionCode",
                table: "Tickets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "Tickets",
                type: "numeric(12,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PromotionCode",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Tickets");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Tickets",
                type: "numeric(12,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)",
                oldNullable: true);
        }
    }
}
