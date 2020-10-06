using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            var usZipCodes = GetUsZipCodes();

            var getStoresTasks = usZipCodes.Select(zipCode => GetStores(zipCode));
            var allStores = await Task.WhenAll(getStoresTasks);

            var uniqueStores = GetUniqueStores(allStores);

            await SaveStores(uniqueStores);
        }

        private static ICollection<string> GetUsZipCodes()
        {
            return File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "uscodes.txt"))
                .Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
        }

        private static async Task<Store[]> GetStores(string usZipCode)
        {
            if (string.IsNullOrWhiteSpace(usZipCode))
                throw new ArgumentNullException(nameof(usZipCode));

            using var client = new HttpClient();

            var response = await client.GetAsync(
                $"https://www.1800petmeds.com/on/demandware.store/Sites-1800petmeds-Site/default/Stores-FindStores?showMap=false&radius=100.0&postalCode={usZipCode}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseRoot>(responseBody);

            return result.Stores;
        }

        private static IEnumerable<Store> GetUniqueStores(Store[][] allStores)
        {
            var storesDictionary = new Dictionary<string, Store>();
            foreach (var stores in allStores)
            {
                foreach (var store in stores)
                {
                    storesDictionary.TryAdd(store.Id, store);
                }
            }

            return storesDictionary.Values;
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
