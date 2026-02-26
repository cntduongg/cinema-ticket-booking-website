using System;
using System.Collections.Generic;

namespace MovieTheater.Web.Areas.MovieManagement.Models
{
    public class PagedResult<T>
    {
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public List<T>  Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
        public bool IsSuccess { get; set; } = true;
    }
}