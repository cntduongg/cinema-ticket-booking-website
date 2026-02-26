public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetScoreAsync(int userId)
    {
        var res = await _httpClient.GetAsync($"/api/score/{userId}");
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<int>();
    }

    public async Task<bool> DeductScoreAsync(int userId, int score)
    {
        var res = await _httpClient.PostAsJsonAsync($"/api/score/{userId}/deduct", score);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> AddScoreAsync(int userId, int score)
    {
        var res = await _httpClient.PostAsJsonAsync($"/api/score/{userId}/add", score);
        return res.IsSuccessStatusCode;
    }
}
