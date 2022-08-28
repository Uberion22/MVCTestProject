using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCTestProject.DataModels
{
    public class CryptocurrencyMetadata
    {
        [Key]
        [ForeignKey("CryptoModel")]
        [JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CryptoId { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date_added")]
        public DateTime? DateAdded { get; set; }

        [JsonProperty("date_launched")]
        public DateTime? DateLaunched { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        public virtual Cryptocurrency CryptoModel { get; set; }
    }
}
