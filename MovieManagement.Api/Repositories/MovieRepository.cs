using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Repositories.IRepositories;
using MovieManagement.Data;


namespace MovieManagement.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;
        public MovieRepository(AppDbContext context) => _context = context;
        public IQueryable<Movie> GetAll() => _context.Movies.AsQueryable();

        public async Task<MovieDurationDTO> getDurationByMovieId(int movieId)
        {
            var movie = _context.Movies.
                        Find(movieId);
            if (movie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movieId} not found.");
            }
            MovieDurationDTO movieDuration = new MovieDurationDTO
            {
                Id = movie.Id,
                MovieName = movie.Name,
                Duration = movie.RunningTime
            };

            return movieDuration;
        }

    }
}
