namespace Generics.Models
{
    public class RequestParametersGets
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Filter { get; set; }
    }
}