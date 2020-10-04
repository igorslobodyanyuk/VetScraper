namespace VetScraper
{
    public class StorePagingObject
    {
        public PagingModel PagingModel { get; set; }
        public MoreContentUrl MoreContentUrl { get; set; }
        public object LessContentUrl { get; set; }
        public PagedStoreList PagedStoreList { get; set; }
        public PageUrl[] PageUrls { get; set; }
    }
}