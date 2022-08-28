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
        private readonly IDatabaseManager<MVCTestProjectContext> _dbManager;

        public CryptocurrencyListingController(IDatabaseManager<MVCTestProjectContext> manager)
        {
            _dbManager = manager;
        }

        // GET: CryptocurrencyList
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
            var usedCultureInfo = System.Globalization.CultureInfo.GetCultureInfo("en-us");
            var cryptocurrencyData = _dbManager.GetCryptocurrencyByFilter(filter, out int totalCount).ToList();
            var result = cryptocurrencyData.Select(m => new CryptocurrencyViewModel()
            {
                Id = m.CryptocurrencyServerId,
                Name = m.Name,
                Symbol = m.Symbol,
                Price = m.Quote.QuoteItem.Price?.ToString("C3", usedCultureInfo),
                PercentChange1h = (m.Quote.QuoteItem.PercentChange1h.GetValueOrDefault() / 100).ToString("P2"),
                VolumeChange24h = (m.Quote.QuoteItem.PercentChange24h.GetValueOrDefault() / 100).ToString("P2"),
                MarketCap = m.Quote.QuoteItem.MarketCap?.ToString("C3", usedCultureInfo),
                LastUpdated = m.LastUpdated,
                Logo = m?.CryptocurrencyMetadata.Logo

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
