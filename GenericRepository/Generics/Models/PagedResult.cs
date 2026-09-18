namespace Generics.Models
{
    public class PagedResult<T> where T : class
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        
        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        
        // =>
        //(int)Math.Ceiling((double)TotalRecords / PageSize);


        public PagedResult()
        {

        }


        public PagedResult(int totalRecords, int currentPage, int pageSize, IEnumerable<T> data)
        {
            TotalRecords = totalRecords;
            TotalPages = totalRecords / pageSize;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Data = data;
        }

        

    }

    
    
}