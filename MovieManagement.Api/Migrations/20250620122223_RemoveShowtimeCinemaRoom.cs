using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveShowtimeCinemaRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CinemaRoom",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Showtime",
                table: "Movies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CinemaRoom",
                table: "Movies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Showtime",
                table: "Movies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room IMAX", "19:00, 21:30" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room A", "18:30, 21:00" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room B", "17:00, 20:00, 22:30" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room C", "18:00, 21:15" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room D", "19:30, 22:00" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room IMAX", "18:00, 21:30" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room E", "17:30, 20:30, 23:00" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room IMAX", "19:00, 22:30" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room F", "16:00, 18:30, 21:00" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CinemaRoom", "Showtime" },
                values: new object[] { "Room G", "18:00, 21:30" });
        }
    }
}
