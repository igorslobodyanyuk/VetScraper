namespace VetScraper
{
    public class ResponseRoot
    {
        public string Action { get; set; }
        public string QueryString { get; set; }
        public string Locale { get; set; }
        public Store[] Stores { get; set; }
        public string Locations { get; set; }
        public SearchKey SearchKey { get; set; }
        public int Radius { get; set; }
        public string ActionUrl { get; set; }
        public string GoogleMapsApi { get; set; }
        public int[] RadiusOptions { get; set; }
        public string StoresResultsHtml { get; set; }
        public StorePagingObject StorePagingObject { get; set; }
    }
}