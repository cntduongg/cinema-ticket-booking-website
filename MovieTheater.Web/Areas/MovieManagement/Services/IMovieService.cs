using System.Threading.Tasks;
using MovieTheater.Web.Areas.MovieManagement.Models;

namespace MovieTheater.Web.Areas.MovieManagement.Services
{
    public interface IMovieService
    {
        Task<PagedResult<MovieViewModel>> GetMoviesAsync(int pageIndex, int pageSize, string search, string sort);

        //Admin
        Task<MovieViewModel> GetMovieByIdAsync(int id);
        Task<bool> AddMovieAsync(MovieViewModel model);
        Task<bool> UpdateMovieAsync(MovieViewModel model);
        Task<bool> DeleteMovieAsync(int id);
    }
}