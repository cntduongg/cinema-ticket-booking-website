using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;

namespace MovieManagement.Api.Repositories.IRepositories
{
    public interface IAdminMovieRepository
    {
        Task<IEnumerable<MovieListDto>> GetAllAsync();
        Task<IEnumerable<AdminMovieListDto>> GetAllAdminAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task AddAsync(MovieCreateDto dto);
        Task UpdateAsync(int id, MovieUpdateDto dto);
        Task<bool> ToggleStatusAsync(int id);
    }
}
