using Newtonsoft.Json;

namespace MVCTestProject.DataModels
{
    public class Quote
    {
        public int Id { get; set; }

        public int CryptocurrencyId { get; set; }
        public int QuoteId { get; set; }

        [JsonProperty("USD")]
        public virtual QuoteItem USD { get; set; }

        public virtual Cryptocurrency Cryptocurrency { get; set; }
    }
}
