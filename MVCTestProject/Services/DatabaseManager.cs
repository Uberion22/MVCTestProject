using Microsoft.EntityFrameworkCore;
using MVCTestProject.Data;
using MVCTestProject.DataModels;

namespace MVCTestProject.Services
{
    public class DatabaseManager: IDatabaseManager<MVCTestProjectContext>
    {
        private readonly MVCTestProjectContext _db;
        private bool _disposed = false;

        public DatabaseManager(MVCTestProjectContext userContext)
        {
            _db = userContext;
        }

        public IQueryable<Cryptocurrency> GetCryptocurrencyList()
        {
            return _db.Cryptocurrencies
                .Include(x => x.Quote.QuoteItem)
                .Include(m => m.CryptocurrencyMetadata);
        }

        public void CreateOrUpdateCryptocurrency(IEnumerable<Cryptocurrency> cryptocurrencies)
        {
            foreach (var cryptocurrency in cryptocurrencies)
            {
                var current = _db.Cryptocurrencies
                    .Include(s => s.Quote.QuoteItem)
                    .Include(p => p.CryptocurrencyMetadata)
                    .FirstOrDefault(i => i.CryptocurrencyServerId == cryptocurrency.CryptocurrencyServerId);
                
                if (current != null)
                {
                    current.Quote = null;
                    current.CryptocurrencyMetadata = null;
                    current.CryptocurrencyMetadata = cryptocurrency.CryptocurrencyMetadata;
                    current.Quote = cryptocurrency.Quote;
                    _db.SaveChanges();
                }
                else
                {
                    _db.Add(cryptocurrency);
                    _db.SaveChanges();
                }
            }
        }

        public IQueryable<Cryptocurrency> GetCryptocurrencyByFilter(CryptocurrencyFilter modelFilter, out int totalCount)
        {
            var result = _db.Cryptocurrencies
                .Include(x => x.Quote.QuoteItem)
                .Include(m => m.CryptocurrencyMetadata)
                .Where(i => i.Quote.QuoteItem.MarketCap >= modelFilter.MarketCap && i.Quote.QuoteItem.Price >= modelFilter.Price);
            
            if (!string.IsNullOrEmpty(modelFilter.Name))
            {
                result = result.Where(i => i.Name.ToUpper().Contains(modelFilter.Name.ToUpper()));
            }

            totalCount = result.Count();

            result = result.Skip(modelFilter.PageSize * (modelFilter.PageNumber - 1))
                .Take(modelFilter.PageSize)
                .OrderByDescending(i => i.Quote.QuoteItem.Price);

            return result;
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string passWord)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == passWord);

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<int> AddUserAsync(User user)
        {
            _db.Users.Add(user);
            return await _db.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
