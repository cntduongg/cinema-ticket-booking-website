namespace MovieTheater.Web.Areas.MovieManagement.Models
{
    public class ApiResult<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
