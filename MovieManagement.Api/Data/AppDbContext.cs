using Microsoft.EntityFrameworkCore;

namespace MovieManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<Movie>().HasData(
    new Movie
    {
        Id = 1,
        Name = "Avatar: The Way of Water",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2022, 12, 16), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2023, 6, 1), DateTimeKind.Utc),
        Actors = "Sam Worthington, Zoe Saldana, Sigourney Weaver",
        ProductionCompany = "20th Century Studios",
        Director = "James Cameron",
        RunningTime = 192,
        Version = "3D IMAX",
        Trailer = "https://www.youtube.com/watch?v=d9MyW72ELq0",
        Type = "Sci-Fi, Adventure",
        Content = "Jake Sully lives with his newfound family formed on the extrasolar moon Pandora.",
        ImagePath = "https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 2,
        Name = "Top Gun: Maverick",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2022, 5, 27), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2023, 8, 1), DateTimeKind.Utc),
        Actors = "Tom Cruise, Miles Teller, Jennifer Connelly",
        ProductionCompany = "Paramount Pictures",
        Director = "Joseph Kosinski",
        RunningTime = 131,
        Version = "2D, 4DX",
        Trailer = "https://www.youtube.com/watch?v=qSqVVswa420",
        Type = "Action, Drama",
        Content = "After thirty years, Maverick is still pushing the envelope as a top naval aviator.",
        ImagePath = "https://image.tmdb.org/t/p/w500/62HCnUTziyWcpDaBO2i1DX17ljH.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 3,
        Name = "Spider-Man: No Way Home",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2021, 12, 17), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2022, 1, 1), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2022, 6, 1), DateTimeKind.Utc),
        Actors = "Tom Holland, Zendaya, Benedict Cumberbatch",
        ProductionCompany = "Sony Pictures",
        Director = "Jon Watts",
        RunningTime = 148,
        Version = "2D, 3D",
        Trailer = "https://www.youtube.com/watch?v=JfVOs4VSpmA",
        Type = "Action, Adventure, Sci-Fi",
        Content = "Spider-Man seeks help from Doctor Strange when his secret identity is revealed.",
        ImagePath = "https://image.tmdb.org/t/p/w500/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 4,
        Name = "Black Panther: Wakanda Forever",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2022, 11, 11), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2023, 5, 1), DateTimeKind.Utc),
        Actors = "Angela Bassett, Letitia Wright, Lupita Nyong'o",
        ProductionCompany = "Marvel Studios",
        Director = "Ryan Coogler",
        RunningTime = 161,
        Version = "2D, IMAX",
        Trailer = "https://www.youtube.com/watch?v=_Z3QKkl1WyM",
        Type = "Action, Adventure, Drama",
        Content = "The people of Wakanda fight to protect their home from intervening world powers.",
        ImagePath = "https://image.tmdb.org/t/p/w500/sv1xJUazXeYqALzczSZ3O6nkH75.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 5,
        Name = "The Batman",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2022, 3, 4), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2022, 3, 10), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2022, 8, 10), DateTimeKind.Utc),
        Actors = "Robert Pattinson, Zoë Kravitz, Paul Dano",
        ProductionCompany = "Warner Bros.",
        Director = "Matt Reeves",
        RunningTime = 176,
        Version = "2D, IMAX",
        Trailer = "https://www.youtube.com/watch?v=mqqft2x_Aa4",
        Type = "Action, Crime, Drama",
        Content = "Batman ventures into Gotham City's underworld when a sadistic killer leaves behind a trail of cryptic clues.",
        ImagePath = "https://image.tmdb.org/t/p/w500/b0PlSFdDwbyK0cf5RxwDpaOJQvQ.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 6,
        Name = "Dune",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2021, 10, 22), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2021, 10, 25), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2022, 3, 25), DateTimeKind.Utc),
        Actors = "Timothée Chalamet, Rebecca Ferguson, Oscar Isaac",
        ProductionCompany = "Warner Bros.",
        Director = "Denis Villeneuve",
        RunningTime = 155,
        Version = "2D, IMAX",
        Trailer = "https://www.youtube.com/watch?v=8g18jFHCLXk",
        Type = "Sci-Fi, Adventure",
        Content = "Paul Atreides leads nomadic tribes in a war against the enemies of his family.",
        ImagePath = "https://image.tmdb.org/t/p/w500/d5NXSklXo0qyIYkgV94XAgMIckC.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 7,
        Name = "Fast X",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2023, 5, 19), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 5, 20), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2023, 10, 20), DateTimeKind.Utc),
        Actors = "Vin Diesel, Michelle Rodriguez, Jason Momoa",
        ProductionCompany = "Universal Pictures",
        Director = "Louis Leterrier",
        RunningTime = 141,
        Version = "2D, 4DX",
        Trailer = "https://www.youtube.com/watch?v=32RAq6JzY-w",
        Type = "Action, Adventure, Crime",
        Content = "Dom Toretto and his family are targeted by the vengeful son of drug kingpin Hernan Reyes.",
        ImagePath = "https://image.tmdb.org/t/p/w500/fiVW06jE7z9YnO4trhaMEdclSiC.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 8,
        Name = "Oppenheimer",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2023, 7, 21), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 7, 22), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2024, 1, 22), DateTimeKind.Utc),
        Actors = "Cillian Murphy, Emily Blunt, Matt Damon",
        ProductionCompany = "Universal Pictures",
        Director = "Christopher Nolan",
        RunningTime = 180,
        Version = "70mm IMAX",
        Trailer = "https://www.youtube.com/watch?v=uYPbbksJxIg",
        Type = "Biography, Drama, History",
        Content = "The story of American scientist J. Robert Oppenheimer and his role in the development of the atomic bomb.",
        ImagePath = "https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 9,
        Name = "Barbie",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2023, 7, 21), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 7, 22), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2024, 2, 22), DateTimeKind.Utc),
        Actors = "Margot Robbie, Ryan Gosling, America Ferrera",
        ProductionCompany = "Warner Bros.",
        Director = "Greta Gerwig",
        RunningTime = 114,
        Version = "2D, Pink Format",
        Trailer = "https://www.youtube.com/watch?v=pBk4NYhWNMM",
        Type = "Comedy, Adventure, Fantasy",
        Content = "Barbie and Ken are having the time of their lives in the colorful and seemingly perfect world of Barbie Land.",
        ImagePath = "https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    },
    new Movie
    {
        Id = 10,
        Name = "John Wick: Chapter 4",
        ReleaseDate = DateTime.SpecifyKind(new DateTime(2023, 3, 24), DateTimeKind.Utc),
        FromDate = DateTime.SpecifyKind(new DateTime(2023, 3, 25), DateTimeKind.Utc),
        ToDate = DateTime.SpecifyKind(new DateTime(2023, 9, 25), DateTimeKind.Utc),
        Actors = "Keanu Reeves, Donnie Yen, Bill Skarsgård",
        ProductionCompany = "Lionsgate",
        Director = "Chad Stahelski",
        RunningTime = 169,
        Version = "2D, 4DX",
        Trailer = "https://www.youtube.com/watch?v=qEVUtrk8_B4",
        Type = "Action, Crime, Thriller",
        Content = "John Wick uncovers a path to defeating The High Table, but he must face a new enemy.",
        ImagePath = "https://image.tmdb.org/t/p/w500/vZloFAK7NmvMGKE7VkF5UHaz0I.jpg",
        Status = true,
        CreatedById = 1,
        CreatedDate = DateTime.SpecifyKind(new DateTime(2025, 6, 17), DateTimeKind.Utc)
    }
);

        }
    }
}
