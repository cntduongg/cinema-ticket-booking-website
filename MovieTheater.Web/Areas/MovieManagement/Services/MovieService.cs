using Humanizer;
using MovieTheater.Web.Areas.MovieManagement.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using System.Threading.Tasks;

namespace MovieTheater.Web.Areas.MovieManagement.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _http;

        public MovieService(HttpClient httpClient)
        {
            _http = httpClient;
            _http.BaseAddress = new Uri("https://localhost:7214/");
        }

        public async Task<PagedResult<MovieViewModel>> GetMoviesAsync(
            int pageIndex, int pageSize, string search, string sort)
        {
            // Gọi API thật
            var data = await _http.GetFromJsonAsync<List<MovieResponseDto>>("api/admin/movies/GetAll") ?? new List<MovieResponseDto>();

            // Map DTO -> ViewModel
            var all = data.Select(d => new MovieViewModel
            {
                Id = d.Id,
                Name = d.Name,
                ReleaseDate = d.ReleaseDate,
                ProductionCompany = d.ProductionCompany,
                RunningTime = d.RunningTime,
                Version = d.Version,
                ImagePath = d.ImagePath,
                Trailer = d.Trailer,
                Status = d.Status,
                Type = d.Type
            });

            // Filter & sort & paginate FE-side
            var filtered = string.IsNullOrWhiteSpace(search)
                ? all
                : all.Where(m => m.Name
                    .Contains(search, StringComparison.OrdinalIgnoreCase));

            var sorted = sort switch
            {
                "ReleaseDate" => filtered.OrderByDescending(m => m.ReleaseDate),
                _ => filtered.OrderBy(m => m.Name)
            };

            var total = sorted.Count();
            var items = sorted
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<MovieViewModel>
            {
                Items = items,
                TotalItems = total,
                PageSize = pageSize,
                CurrentPage = pageIndex
            };
        }

        public async Task<MovieViewModel> GetMovieByIdAsync(int id)
        {
            var dto = await _http.GetFromJsonAsync<MovieViewModel>($"api/admin/movies/GetById/{id}");
            if (dto == null) return null;
            return new MovieViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                ReleaseDate = dto.ReleaseDate,
                Actors = dto.Actors,
                ProductionCompany = dto.ProductionCompany,
                Director = dto.Director,
                RunningTime = dto.RunningTime,
                Version = dto.Version,
                Trailer = dto.Trailer,
                Type = dto.Type,

                Content = dto.Content,
                ImagePath = dto.ImagePath,
                Status = dto.Status
            };
        }

        public async Task<bool> AddMovieAsync(MovieViewModel model)
        {
            // Create options for JSON serialization with custom date formatting
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
            {
                new JsonStringEnumConverter(),
                new DateTimeJsonConverter()
            }
            };

            var json = JsonSerializer.Serialize(model, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/admin/movies/Create", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateMovieAsync(MovieViewModel model)
        {
            // Use the same options for update
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
            {
                    new JsonStringEnumConverter(),
                    new DateTimeJsonConverter()
            }
            };

            var json = JsonSerializer.Serialize(model, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"api/admin/movies/Update/{model.Id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var response = await _http.PatchAsync($"api/admin/movies/{id}/toggle-status", null);
            return response.IsSuccessStatusCode;
        }

        private class DateTimeJsonConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.Parse(reader.GetString()!);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                // Format as ISO 8601 with milliseconds and 'Z' for UTC
                writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'"));
            }
        }

    }
}
