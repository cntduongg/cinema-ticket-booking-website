using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;
using MovieManagement.Api.Repositories.IRepositories;
using MovieManagement.Api.Services.IServices;

namespace MovieManagement.Api.Repositories
{
    public class AdminMovieRepository : IAdminMovieRepository
    {
        private readonly IAdminMovieService _service;

        public AdminMovieRepository(IAdminMovieService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<MovieListDto>> GetAllAsync()
        {
            return await _service.GetAllMoviesAsync();
        }
        public async Task<IEnumerable<AdminMovieListDto>> GetAllAdminAsync()
        {
            return await _service.GetAllMoviesAdminAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        public async Task AddAsync(MovieCreateDto dto)
        {
            await _service.AddMovieFromDtoAsync(dto);
        }

        public async Task UpdateAsync(int id, MovieUpdateDto dto)
        {
            await _service.UpdateMovieFromDtoAsync(id, dto);
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            return await _service.ToggleStatusAsync(id);
        }
    }
}
