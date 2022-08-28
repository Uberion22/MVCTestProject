using Microsoft.AspNetCore.Mvc.Rendering;
using MVCTestProject.DataModels;

namespace MVCTestProject.ViewModels.Cryptocurrency
{
    public class CryptocurrencyListViewModel
    {
        public IEnumerable<CryptocurrencyViewModel> CryptocurrencyList { get; set; }

        public CryptocurrencyFilter ModelFilter { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}