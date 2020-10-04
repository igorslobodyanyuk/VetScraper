namespace VetScraper
{
    public class Store
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public object Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public float Distance { get; set; }
        public bool HideOnDirectory { get; set; }
    }
}