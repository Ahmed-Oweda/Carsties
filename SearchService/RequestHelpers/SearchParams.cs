namespace SearchService.RequestHelpers
{
    public class SearchParams
    {
        public string Seller { get; internal set; }
        public string Winner { get; internal set; }
        public int PageNumber { get; internal set; }
        public int PageSize { get; internal set; }
        public string SearchTerm { get; internal set; }
        public string FilterBy { get; set; }
        public string OrderBy { get; set; }
    }
}
