using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Actors = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProductionCompany = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Director = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RunningTime = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Trailer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CinemaRoom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Showtime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ImagePath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Actors", "CinemaRoom", "Content", "Director", "FromDate", "ImagePath", "Name", "ProductionCompany", "ReleaseDate", "RunningTime", "Showtime", "Status", "ToDate", "Trailer", "Type", "Version" },
                values: new object[,]
                {
                    { 1, "John Doe, Jane Smith", "Room A", "An epic adventure movie.", "Mike Johnson", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "images/movies/adventure1.jpg", "The Great Adventure", "Adventure Studios", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 120, "18:00", true, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://youtube.com/trailer1", "Adventure, Action", "2D" },
                    { 2, "Alice Brown, Bob White", "Room B", "A heartfelt romantic story.", "Laura Green", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), "images/movies/romanticsunset.jpg", "Romantic Sunset", "Romance Films", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 105, "20:30", true, new DateTime(2024, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), "https://youtube.com/trailer2", "Romance, Drama", "3D" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Name",
                table: "Movies",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
