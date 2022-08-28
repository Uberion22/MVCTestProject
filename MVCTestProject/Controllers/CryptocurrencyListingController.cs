using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCTestProject.Data;
using MVCTestProject.DataModels;
using MVCTestProject.Services;
using MVCTestProject.ViewModels.Cryptocurrency;

namespace MVCTestProject.Controllers
{
    [Authorize]
    public class CryptocurrencyListingController : Controller
    {
        private readonly IDatabaseManager<UserContext> _dbManager;

        public CryptocurrencyListingController(IDatabaseManager<UserContext> manager)
        {
            _dbManager = manager;
        }

        // GET: CryptoViewModels
        public IActionResult Index(string name, double price, double marketCup, int page = 1)
        {
            var pageSize = 20;
            var filter = new CryptocurrencyFilter()
            {
                Name = name,
                Price = price,
                MarketCap = marketCup,
                PageNumber = page,
                PageSize = pageSize,
            };
            var cryptocurrencyData = _dbManager.GetCryptocurrencyByFilter(filter, out int totalCount).ToList();
            var result = cryptocurrencyData.Select(m => new CryptocurrencyViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Symbol = m.Symbol,
                Price = m.Quote.USD.Price,
                PercentChange1h = m.Quote.USD.PercentChange1h,
                VolumeChange24h = m.Quote.USD.VolumeChange24h,
                MarketCap = m.Quote.USD.MarketCap,
                LastUpdated = m.LastUpdated,
                Logo = m.Metadata.Logo

            });
            PageViewModel pageViewModel = new(totalCount, page, pageSize);
            var view = new CryptocurrencyListViewModel()
            {
                CryptocurrencyList = result,
                ModelFilter = filter,
                PageViewModel = pageViewModel
            };

            return View(view);
        }
    }
}
