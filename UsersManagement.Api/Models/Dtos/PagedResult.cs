namespace UsersManagement.Api.Models.Dtos
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
        public int Size { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public string Message { get; set; }
    }
}