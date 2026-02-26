using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Movie;


namespace MovieManagement.Api.Services.IServices
{
    public interface IMovieService
    {
        Task<List<UserMovieListDto>> SearchByNameAsync(string keyword);
        Task<List<UserMovieListDto>> SortByNameAsync();
        Task<List<UserMovieListDto>> FilterByTypeAsync(string type);

        Task<MovieDurationDTO> GetDurationByMovieId(int movieId);

    }
}
