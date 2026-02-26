using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Actors", "CinemaRoom", "Content", "CreatedById", "CreatedDate", "Director", "FromDate", "ImagePath", "Name", "ProductionCompany", "ReleaseDate", "RunningTime", "Showtime", "ToDate", "Trailer", "Type", "Version" },
                values: new object[] { "Sam Worthington, Zoe Saldana, Sigourney Weaver", "Room IMAX", "Jake Sully lives with his newfound family formed on the extrasolar moon Pandora.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "James Cameron", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg", "Avatar: The Way of Water", "20th Century Studios", new DateTime(2022, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 192, "19:00, 21:30", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=d9MyW72ELq0", "Sci-Fi, Adventure", "3D IMAX" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Actors", "CinemaRoom", "Content", "CreatedById", "CreatedDate", "Director", "FromDate", "ImagePath", "Name", "ProductionCompany", "ReleaseDate", "RunningTime", "Showtime", "ToDate", "Trailer", "Type", "Version" },
                values: new object[] { "Tom Cruise, Miles Teller, Jennifer Connelly", "Room A", "After thirty years, Maverick is still pushing the envelope as a top naval aviator.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Joseph Kosinski", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/62HCnUTziyWcpDaBO2i1DX17ljH.jpg", "Top Gun: Maverick", "Paramount Pictures", new DateTime(2022, 5, 27, 0, 0, 0, 0, DateTimeKind.Utc), 131, "18:30, 21:00", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=qSqVVswa420", "Action, Drama", "2D, 4DX" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Actors", "CinemaRoom", "Content", "CreatedById", "CreatedDate", "Director", "FromDate", "ImagePath", "Name", "ProductionCompany", "ReleaseDate", "RunningTime", "Showtime", "Status", "ToDate", "Trailer", "Type", "Version" },
                values: new object[,]
                {
                    { 3, "Tom Holland, Zendaya, Benedict Cumberbatch", "Room B", "Spider-Man seeks help from Doctor Strange when his secret identity is revealed.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Jon Watts", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg", "Spider-Man: No Way Home", "Sony Pictures", new DateTime(2021, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 148, "17:00, 20:00, 22:30", true, new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=JfVOs4VSpmA", "Action, Adventure, Sci-Fi", "2D, 3D" },
                    { 4, "Angela Bassett, Letitia Wright, Lupita Nyong'o", "Room C", "The people of Wakanda fight to protect their home from intervening world powers.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Ryan Coogler", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/sv1xJUazXeYqALzczSZ3O6nkH75.jpg", "Black Panther: Wakanda Forever", "Marvel Studios", new DateTime(2022, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), 161, "18:00, 21:15", true, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=_Z3QKkl1WyM", "Action, Adventure, Drama", "2D, IMAX" },
                    { 5, "Robert Pattinson, Zoë Kravitz, Paul Dano", "Room D", "Batman ventures into Gotham City's underworld when a sadistic killer leaves behind a trail of cryptic clues.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Matt Reeves", new DateTime(2022, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/b0PlSFdDwbyK0cf5RxwDpaOJQvQ.jpg", "The Batman", "Warner Bros.", new DateTime(2022, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), 176, "19:30, 22:00", true, new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=mqqft2x_Aa4", "Action, Crime, Drama", "2D, IMAX" },
                    { 6, "Timothée Chalamet, Rebecca Ferguson, Oscar Isaac", "Room IMAX", "Paul Atreides leads nomadic tribes in a war against the enemies of his family.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Denis Villeneuve", new DateTime(2021, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/d5NXSklXo0qyIYkgV94XAgMIckC.jpg", "Dune", "Warner Bros.", new DateTime(2021, 10, 22, 0, 0, 0, 0, DateTimeKind.Utc), 155, "18:00, 21:30", true, new DateTime(2022, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=8g18jFHCLXk", "Sci-Fi, Adventure", "2D, IMAX" },
                    { 7, "Vin Diesel, Michelle Rodriguez, Jason Momoa", "Room E", "Dom Toretto and his family are targeted by the vengeful son of drug kingpin Hernan Reyes.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Louis Leterrier", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/fiVW06jE7z9YnO4trhaMEdclSiC.jpg", "Fast X", "Universal Pictures", new DateTime(2023, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), 141, "17:30, 20:30, 23:00", true, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=32RAq6JzY-w", "Action, Adventure, Crime", "2D, 4DX" },
                    { 8, "Cillian Murphy, Emily Blunt, Matt Damon", "Room IMAX", "The story of American scientist J. Robert Oppenheimer and his role in the development of the atomic bomb.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Christopher Nolan", new DateTime(2023, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg", "Oppenheimer", "Universal Pictures", new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Utc), 180, "19:00, 22:30", true, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=uYPbbksJxIg", "Biography, Drama, History", "70mm IMAX" },
                    { 9, "Margot Robbie, Ryan Gosling, America Ferrera", "Room F", "Barbie and Ken are having the time of their lives in the colorful and seemingly perfect world of Barbie Land.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Greta Gerwig", new DateTime(2023, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg", "Barbie", "Warner Bros.", new DateTime(2023, 7, 21, 0, 0, 0, 0, DateTimeKind.Utc), 114, "16:00, 18:30, 21:00", true, new DateTime(2024, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=pBk4NYhWNMM", "Comedy, Adventure, Fantasy", "2D, Pink Format" },
                    { 10, "Keanu Reeves, Donnie Yen, Bill Skarsgård", "Room G", "John Wick uncovers a path to defeating The High Table, but he must face a new enemy.", 1, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Chad Stahelski", new DateTime(2023, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), "https://image.tmdb.org/t/p/w500/vZloFAK7NmvMGKE7VkF5UHaz0I.jpg", "John Wick: Chapter 4", "Lionsgate", new DateTime(2023, 3, 24, 0, 0, 0, 0, DateTimeKind.Utc), 169, "18:00, 21:30", true, new DateTime(2023, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), "https://www.youtube.com/watch?v=qEVUtrk8_B4", "Action, Crime, Thriller", "2D, 4DX" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Actors", "CinemaRoom", "Content", "CreatedById", "CreatedDate", "Director", "FromDate", "ImagePath", "Name", "ProductionCompany", "ReleaseDate", "RunningTime", "Showtime", "ToDate", "Trailer", "Type", "Version" },
                values: new object[] { "John Doe, Jane Smith", "Room A", "An epic adventure movie.", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mike Johnson", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "images/movies/adventure1.jpg", "The Great Adventure", "Adventure Studios", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 120, "18:00", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "https://youtube.com/trailer1", "Adventure, Action", "2D" });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Actors", "CinemaRoom", "Content", "CreatedById", "CreatedDate", "Director", "FromDate", "ImagePath", "Name", "ProductionCompany", "ReleaseDate", "RunningTime", "Showtime", "ToDate", "Trailer", "Type", "Version" },
                values: new object[] { "Alice Brown, Bob White", "Room B", "A heartfelt romantic story.", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laura Green", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), "images/movies/romanticsunset.jpg", "Romantic Sunset", "Romance Films", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 105, "20:30", new DateTime(2024, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), "https://youtube.com/trailer2", "Romance, Drama", "3D" });
        }
    }
}
