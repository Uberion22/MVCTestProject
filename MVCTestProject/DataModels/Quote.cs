using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCTestProject.DataModels
{
    public class Quote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CryprocurrencyId { get; set; }

        public virtual Cryptocurrency Cryptocurrency { get; set; }

        [JsonProperty("USD")]
        public virtual QuoteItem QuoteItem { get; set; }
    }
}
