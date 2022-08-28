namespace MVCTestProject.DataModels
{
    public class CryptocurrencyFilter
    {
        public string Name { get; set; }
        
        public double Price { get; set; }
        
        public double MarketCap { get; set; }
       
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
    }
}
