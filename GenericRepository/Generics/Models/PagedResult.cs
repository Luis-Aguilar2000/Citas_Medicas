using System.Linq.Expressions;
namespace Generics.Models

{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}