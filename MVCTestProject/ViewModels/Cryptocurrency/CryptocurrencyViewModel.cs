using System.ComponentModel;

namespace MVCTestProject.ViewModels.Cryptocurrency
{
    public class CryptocurrencyViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Символ")]
        public string Symbol { get; set; }

        [DisplayName("Логотип")]
        public string Logo { get; set; }

        [DisplayName("Текущая цена в USD")]
        public string Price { get; set; }

        [DisplayName("Изменение цены за 1ч")]
        public string PercentChange1h { get; set; }

        [DisplayName("Изменение цены за 24ч")]
        public string VolumeChange24h { get; set; }

        [DisplayName("Капитализация в USD")]
        public string MarketCap { get; set; }

        [DisplayName("Время обновления данных")]
        public DateTime LastUpdated { get; set; }
    }
}
