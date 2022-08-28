using MVCTestProject.Data;
using MVCTestProject.DataModels;
using Newtonsoft.Json;
using System.Web;

namespace MVCTestProject.Services
{
    public class DataLoaderService : IHostedService, IDisposable
    {
        private const string MY_KEY = "bbc538ad-279e-4af9-8789-6f8a5899da8d";
        private const string LATEST_CRYPTOCURRENCY_END_POINT = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";
        private const string METADATA_END_POINT = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/info";
        private static Timer _timer;
        private readonly int _interval = 60000; //60 секунд
        private readonly IServiceProvider _serviceProvider;      
        private IDatabaseManager<UserContext> _databaseManager;

        public DataLoaderService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void UpdateCryptocurrencyInDatabase(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            _databaseManager = scope.ServiceProvider.GetService<IDatabaseManager<UserContext>>();
            var cryptocurrencyDTOList = GetCryptocurrencyList().Result;
            var idList = cryptocurrencyDTOList.Select(i => i.Id).ToList();
            var cryptocurrencyMetadataDTOList = GetCryptocurrencyMetaDataList(idList).Result;
            _databaseManager.CreateOrUpdateCryptocurrency(cryptocurrencyDTOList);
            _databaseManager.CreateOrUpdateCryptocurrencyMetadata(cryptocurrencyMetadataDTOList);
        }

        private async Task<List<Cryptocurrency>> GetCryptocurrencyList()
        {
            var URL = new UriBuilder(LATEST_CRYPTOCURRENCY_END_POINT);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "10";
            queryString["convert"] = "USD";
            URL.Query = queryString.ToString();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", MY_KEY);
            client.DefaultRequestHeaders.Add("Accepts", "application/json");
            var resultString = await client.GetStringAsync(URL.ToString());
            var cryptocurrencyList = ConvertJsonToCryptocurrencyList(resultString);
            
            return cryptocurrencyList;
        }

        public async Task<List<CryptocurrencyMetadata>> GetCryptocurrencyMetaDataList(List<int> idList)
        {
            var URL = new UriBuilder(METADATA_END_POINT);
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = String.Join(',',idList);
            URL.Query = queryString.ToString();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", MY_KEY);
            client.DefaultRequestHeaders.Add("Accepts", "application/json");
            string result = await client.GetStringAsync(URL.ToString());
            var models = ConvertJsonToCryptocurrencyMetadataList(result);

            return models;
        }

        public static List<Cryptocurrency> ConvertJsonToCryptocurrencyList(string json)
        {
            var objects = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            var result = new List<Cryptocurrency>();

            if (objects.TryGetValue("data", out var data))
            {
                result = JsonConvert.DeserializeObject<List<Cryptocurrency>>(data.ToString());
            }

            return result;
        }

        public static List<CryptocurrencyMetadata> ConvertJsonToCryptocurrencyMetadataList(string json)
        {
            var objects = (JsonConvert.DeserializeObject<Dictionary<string, object>>(json));
            var result = new List<CryptocurrencyMetadata>();

            if (objects.TryGetValue("data", out var data))
            {
                result = JsonConvert.DeserializeObject<Dictionary<int, CryptocurrencyMetadata>>(data.ToString()).Values.ToList();
            }

            return result;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateCryptocurrencyInDatabase, null, 0, _interval);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
