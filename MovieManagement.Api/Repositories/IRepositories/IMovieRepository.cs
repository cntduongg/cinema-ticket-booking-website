using MovieManagement.Api.Models.DTO;


namespace MovieManagement.Api.Repositories.IRepositories
{
    public interface IMovieRepository
    {
        IQueryable<Movie> GetAll();
        Task<MovieDurationDTO> getDurationByMovieId(int movieId);
    }
}
