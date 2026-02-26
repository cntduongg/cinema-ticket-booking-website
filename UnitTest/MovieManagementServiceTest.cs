using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using MovieManagement.Api.Models;
using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;
using MovieManagement.Api.Models.DTO.Movie;
using MovieManagement.Api.Repositories.IRepositories;
using MovieManagement.Api.Services;
using MovieManagement.Data;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace UnitTest
{
    using SeverityLevel = Allure.Net.Commons.SeverityLevel;

    /// <summary>
    /// Unit tests for MovieService using mocked IMovieRepository
    /// </summary>
    [AllureSuite("MovieService Unit Tests")]
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieService = new MovieService(_movieRepositoryMock.Object);
        }

        [Fact]
        [AllureFeature("Search")]
        [AllureStory("Search by Name")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "search")]
        public async Task SearchByNameAsync_WithValidKeyword_ShouldReturnMatchingMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Name = "Java Developer Movie",
                    Status = true,
                    ReleaseDate = DateTime.Now.AddMonths(-6),
                    Actors = "Robert Downey Jr.",
                    Director = "Russo Brothers",
                    RunningTime = 180,
                    Trailer = "https://trailer1.com",
                    Type = "Action",
                    ImagePath = "/images/avengers.jpg",
                    Content = "Epic superhero movie"
                },
                new Movie
                {
                    Id = 2,
                    Name = "Avatar: The Way of Water",
                    Status = true,
                    ReleaseDate = DateTime.Now.AddMonths(-3),
                    Actors = "Sam Worthington",
                    Director = "James Cameron",
                    RunningTime = 190,
                    Trailer = "https://trailer2.com",
                    Type = "Sci-Fi",
                    ImagePath = "/images/avatar.jpg",
                    Content = "Pandora adventure continues"
                },
                new Movie
                {
                    Id = 3,
                    Name = "Batman Returns",
                    Status = true,
                    ReleaseDate = DateTime.Now.AddMonths(-12),
                    Actors = "Michael Keaton",
                    Director = "Tim Burton",
                    RunningTime = 126,
                    Trailer = "https://trailer3.com",
                    Type = "Action",
                    ImagePath = "/images/batman.jpg",
                    Content = "Dark knight returns"
                }
            };

            var mockQueryable = movies.AsQueryable().BuildMock();
            _movieRepositoryMock.Setup(r => r.GetAll()).Returns(mockQueryable);

            // Act
            var result = await _movieService.SearchByNameAsync("ava");

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(m => m.Name.ToLower().Contains("ava"));
            result.Select(m => m.Name).Should().BeInAscendingOrder();
        }

        [Fact]
        [AllureFeature("Search")]
        [AllureStory("Search by Name with Empty Keyword")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "search")]
        public async Task SearchByNameAsync_WithEmptyKeyword_ShouldReturnAllMovies()
        {
            // Arrange
            var movies = CreateTestMovies();
            var mockQueryable = movies.AsQueryable().BuildMock();
            _movieRepositoryMock.Setup(r => r.GetAll()).Returns(mockQueryable);

            // Act
            var result = await _movieService.SearchByNameAsync("");

            // Assert
            result.Should().HaveCount(3);
            result.Select(m => m.Name).Should().BeInAscendingOrder();
        }

        [Fact]
        [AllureFeature("Sorting")]
        [AllureStory("Sort movies by name")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "sort")]
        public async Task SortByNameAsync_ShouldReturnMoviesSortedAlphabetically()
        {
            // Arrange
            var movies = new List<Movie>
            {
                CreateTestMovie(1, "Zorro", DateTime.Now),
                CreateTestMovie(2, "Avengers", DateTime.Now),
                CreateTestMovie(3, "Batman", DateTime.Now)
            };

            var mockQueryable = movies.AsQueryable().BuildMock();
            _movieRepositoryMock.Setup(r => r.GetAll()).Returns(mockQueryable);

            // Act
            var result = await _movieService.SortByNameAsync();

            // Assert
            result.Should().HaveCount(3);
            result.First().Name.Should().Be("Avengers");
            result.Last().Name.Should().Be("Zorro");
            result.Select(m => m.Name).Should().BeInAscendingOrder();
        }

        [Theory]
        [InlineData("dang-chieu")]
        [AllureFeature("Filtering")]
        [AllureStory("Filter currently showing movies")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "filter")]
        public async Task FilterByTypeAsync_WithCurrentlyShowingType_ShouldReturnCurrentMovies(string filterType)
        {
            // Arrange
            var now = DateTime.Now;
            var movies = new List<Movie>
            {
                CreateTestMovie(1, "Current Movie", now, now.AddDays(-5), now.AddDays(5)),
                CreateTestMovie(2, "Upcoming Movie", now, now.AddDays(2), now.AddDays(10)),
                CreateTestMovie(3, "Past Movie", now, now.AddDays(-20), now.AddDays(-5))
            };

            var mockQueryable = movies.AsQueryable().BuildMock();
            _movieRepositoryMock.Setup(r => r.GetAll()).Returns(mockQueryable);

            // Act
            var result = await _movieService.FilterByTypeAsync(filterType);

            // Assert
            result.Should().ContainSingle();
            result.First().Name.Should().Be("Current Movie");
        }

        [Theory]
        [InlineData("sap-chieu")]
        [AllureFeature("Filtering")]
        [AllureStory("Filter upcoming movies")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "filter")]
        public async Task FilterByTypeAsync_WithUpcomingType_ShouldReturnUpcomingMovies(string filterType)
        {
            // Arrange
            var now = DateTime.Now;
            var movies = new List<Movie>
            {
                CreateTestMovie(1, "Current Movie", now, now.AddDays(-5), now.AddDays(5)),
                CreateTestMovie(2, "Upcoming Movie", now, now.AddDays(2), now.AddDays(10)),
                CreateTestMovie(3, "Another Upcoming", now, now.AddDays(7), now.AddDays(15))
            };

            var mockQueryable = movies.AsQueryable().BuildMock();
            _movieRepositoryMock.Setup(r => r.GetAll()).Returns(mockQueryable);

            // Act
            var result = await _movieService.FilterByTypeAsync(filterType);

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(m => m.Name.Contains("Upcoming"));
        }

        [Fact]
        [AllureFeature("Duration")]
        [AllureStory("Get movie duration by ID")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "duration")]
        public async Task GetDurationByMovieId_ShouldReturnMovieDuration()
        {
            // Arrange
            var expectedDuration = new MovieDurationDTO { MovieName = "Test Movie" };
            _movieRepositoryMock.Setup(r => r.getDurationByMovieId(1))
                              .ReturnsAsync(expectedDuration);

            // Act
            var result = await _movieService.GetDurationByMovieId(1);

            // Assert
            result.Should().NotBeNull();
            result.MovieName.Should().Be("Test Movie");
            _movieRepositoryMock.Verify(r => r.getDurationByMovieId(1), Times.Once);
        }

        private List<Movie> CreateTestMovies()
        {
            return new List<Movie>
            {
                CreateTestMovie(1, "Avengers", DateTime.Now),
                CreateTestMovie(2, "Batman", DateTime.Now),
                CreateTestMovie(3, "Superman", DateTime.Now)
            };
        }

        private Movie CreateTestMovie(int id, string name, DateTime releaseDate,
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            return new Movie
            {
                Id = id,
                Name = name,
                Status = true,
                ReleaseDate = releaseDate,
                FromDate = fromDate ?? DateTime.Now.AddDays(-1),
                ToDate = toDate ?? DateTime.Now.AddDays(30),
                Actors = "Test Actor",
                Director = "Test Director",
                RunningTime = 120,
                Trailer = "https://test-trailer.com",
                Type = "Action",
                ImagePath = "/images/test.jpg",
                Content = "Test movie content",
                ProductionCompany = "Test Studio",
                Version = "2D",
                CreatedById = 1,
                CreatedDate = DateTime.Now
            };
        }
    }

    /// <summary>
    /// Integration tests for AdminMovieService using InMemory database
    /// </summary>
    [AllureSuite("AdminMovieService Integration Tests")]
    public class AdminMovieServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AdminMovieService _adminMovieService;
        private readonly string _databaseName;

        public AdminMovieServiceTests()
        {
            _databaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(_databaseName)
                .Options;

            _context = new AppDbContext(options);
            _adminMovieService = new AdminMovieService(_context);

            SeedTestData();
        }

        [Fact]
        [AllureFeature("Get All Movies")]
        [AllureStory("Return all movies including inactive")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("integration", "movies")]
        public async Task GetAllMoviesAsync_ShouldReturnAllMoviesIncludingInactive()
        {
            // Act
            var result = await _adminMovieService.GetAllMoviesAsync();

            // Assert
            result.Should().HaveCount(4);
            result.Should().Contain(m => m.Name == "Active Movie 1");
            result.Should().Contain(m => m.Name == "Active Movie 2");
            result.Should().Contain(m => m.Name == "Inactive Movie");
            result.Should().Contain(m => m.Name == "Upcoming Movie");
        }

        [Fact]
        [AllureFeature("Get All Movies Admin")]
        [AllureStory("Return only active movies")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("integration", "movies")]
        public async Task GetAllMoviesAdminAsync_ShouldReturnOnlyActiveMovies()
        {
            // Act
            var result = await _adminMovieService.GetAllMoviesAdminAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().OnlyContain(m => m.Name != "Inactive Movie");
            result.Should().AllSatisfy(m => m.Id.Should().BePositive());
        }

        [Fact]
        [AllureFeature("Get Movie By Id")]
        [AllureStory("Return movie with valid Id")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("integration", "movie")]
        public async Task GetByIdAsync_WithValidId_ShouldReturnMovie()
        {
            // Act
            var result = await _adminMovieService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Active Movie 1");
            result.Status.Should().BeTrue();
        }

        [Fact]
        [AllureFeature("Get Movie By Id")]
        [AllureStory("Return null with invalid Id")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("integration", "movie")]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _adminMovieService.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        [AllureFeature("Tracking")]
        [AllureStory("Get entity with tracking")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("integration", "tracking")]
        public async Task GetWithTrackingByIdAsync_ShouldReturnTrackedEntity()
        {
            // Act
            var result = await _adminMovieService.GetWithTrackingByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Active Movie 1");

            // Verify entity is tracked by changing a property
            var originalName = result.Name;
            result.Name = "Modified Name";

            // The change should be reflected without explicit update call
            _context.Entry(result).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        [AllureFeature("Add Movie")]
        [AllureStory("Add movie from DTO")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("integration", "add")]
        public async Task AddMovieFromDtoAsync_ShouldCreateNewMovie()
        {
            // Arrange
            var createDto = new MovieCreateDto
            {   
                Name = "New Test Movie",
                ReleaseDate = DateTime.Now.AddMonths(2),
                FromDate = DateTime.Now.AddMonths(2),
                ToDate = DateTime.Now.AddMonths(3),
                Actors = "New Actor",
                ProductionCompany = "New Studio",
                Director = "New Director",
                RunningTime = 150,
                Version = "IMAX",
                Trailer = "https://new-trailer.com",
                Type = "Drama",
                Content = "New movie content",
                ImagePath = "/images/new-movie.jpg",
                CreatedById = 1,
                CreatedDate = DateTime.Now
            };

            // Act
            await _adminMovieService.AddMovieFromDtoAsync(createDto);

            // Assert
            var movies = await _context.Movies.ToListAsync();
            movies.Should().HaveCount(5);

            var newMovie = movies.FirstOrDefault(m => m.Name == "New Test Movie");
            newMovie.Should().NotBeNull();
            newMovie!.Status.Should().BeTrue();
            newMovie.RunningTime.Should().Be(150);
        }

        [Fact]
        [AllureFeature("Toggle Status")]
        [AllureStory("Toggle movie status by Id")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("integration", "toggle")]
        public async Task ToggleStatusAsync_WithValidId_ShouldToggleStatusAndReturnTrue()
        {
            // Arrange - Movie with Id 3 is inactive
            var movieBefore = await _adminMovieService.GetByIdAsync(3);
            movieBefore!.Status.Should().BeFalse();

            // Act
            var result = await _adminMovieService.ToggleStatusAsync(3);

            // Assert
            result.Should().BeTrue();

            var movieAfter = await _adminMovieService.GetByIdAsync(3);
            movieAfter!.Status.Should().BeTrue();
        }

        [Fact]
        [AllureFeature("Toggle Status")]
        [AllureStory("Toggle movie status with invalid Id")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("integration", "toggle")]
        public async Task ToggleStatusAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _adminMovieService.ToggleStatusAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [AllureFeature("Add Movie")]
        [AllureStory("Add movie manually")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("integration", "add")]
        public async Task AddAsync_ShouldAddMovieToContext()
        {
            // Arrange
            var newMovie = CreateTestMovie("Manual Add Movie");

            // Act
            await _adminMovieService.AddAsync(newMovie);
            await _adminMovieService.SaveChangesAsync();

            // Assert
            var movies = await _context.Movies.ToListAsync();
            movies.Should().HaveCount(5);
            movies.Should().Contain(m => m.Name == "Manual Add Movie");
        }

        [Fact]
        [AllureFeature("Update Movie")]
        [AllureStory("Update existing movie")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("integration", "update")]
        public async Task UpdateAsync_ShouldModifyExistingMovie()
        {
            // Arrange
            var movie = await _adminMovieService.GetWithTrackingByIdAsync(1);
            movie!.Name = "Updated Movie Name";
            movie.RunningTime = 999;

            // Act
            await _adminMovieService.UpdateAsync(movie);
            await _adminMovieService.SaveChangesAsync();

            // Assert
            var updatedMovie = await _adminMovieService.GetByIdAsync(1);
            updatedMovie!.Name.Should().Be("Updated Movie Name");
            updatedMovie.RunningTime.Should().Be(999);
        }

        private void SeedTestData()
        {
            var movies = new List<Movie>
            {
                CreateTestMovie("Active Movie 1", id: 1, status: true),
                CreateTestMovie("Active Movie 2", id: 2, status: true),
                CreateTestMovie("Inactive Movie", id: 3, status: false),
                CreateTestMovie("Upcoming Movie", id: 4, status: true)
            };

            _context.Movies.AddRange(movies);
            _context.SaveChanges();
        }

        private Movie CreateTestMovie(string name, int id = 0, bool status = true)
        {
            return new Movie
            {
                Id = id,
                Name = name,
                Status = status,
                ReleaseDate = DateTime.Now.AddMonths(-1),
                FromDate = DateTime.Now.AddDays(-10),
                ToDate = DateTime.Now.AddDays(20),
                Actors = "Test Actor",
                ProductionCompany = "Test Studio",
                Director = "Test Director",
                RunningTime = 120,
                Version = "2D",
                Trailer = "https://test-trailer.com",
                Type = "Action",
                Content = "Test content",
                ImagePath = "/images/test.jpg",
                CreatedById = 1,
                CreatedDate = DateTime.Now
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}


