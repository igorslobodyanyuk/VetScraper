using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using Newtonsoft.Json;

namespace VetScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stores = await GetStores();

            await SaveStores(stores);
        }

        private static async Task<Store[]> GetStores()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.1800petmeds.com/on/demandware.store/Sites-1800petmeds-Site/default/");

            var response = await client.GetAsync("Stores-FindStores");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseRoot>(responseBody);

            return result.Stores;
        }

        private static async Task SaveStores(IEnumerable<Store> stores)
        {
            await using var fileStream = File.Create(Path.Combine(AppContext.BaseDirectory, "stores.csv"));
            await using var streamWriter = new StreamWriter(fileStream);
            await using var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(stores);
        }
    }
}
