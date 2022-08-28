using Microsoft.EntityFrameworkCore;
using MVCTestProject.Data;
using MVCTestProject.DataModels;

namespace MVCTestProject.Services
{
    public class DatabaseManager: IDatabaseManager<UserContext>
    {
        private readonly UserContext _db;
        private bool _disposed = false;

        public DatabaseManager(UserContext userContext)
        {
            _db = userContext;
        }

        public IQueryable<Cryptocurrency> GetCryptocurrencyList()
        {
            return _db.CryptoModel.Include(x => x.Quote.USD).Include(m => m.Metadata);
        }

        public void CreateOrUpdateCryptocurrency(IEnumerable<Cryptocurrency> cryptocurrencyDTOs)
        {
            foreach (var cryptocurrency in cryptocurrencyDTOs)
            {
                if (_db.CryptoModel.Any(i => i.Id == cryptocurrency.Id))
                {
                    _db.Update(cryptocurrency);
                }
                else
                {
                    _db.Add(cryptocurrency);
                }
            }
            _db.SaveChanges();
        }

        public void CreateOrUpdateCryptocurrencyMetadata(IEnumerable<CryptocurrencyMetadata> cryptocurrencyMetadatas)
        {
            foreach (var metaData in cryptocurrencyMetadatas)
            {
                if (_db.CryptoMetadatas.Any(i => i.CryptoId == metaData.CryptoId))
                {
                    _db.Update(metaData);
                }
                else
                {
                    _db.Add(metaData);
                }
            }
            _db.SaveChanges();
        }

        public IQueryable<Cryptocurrency> GetCryptocurrencyByFilter(CryptocurrencyFilter modelFilter, out int totalCount)
        {
            var result = _db.CryptoModel.Include(x => x.Quote.USD).Include(m => m.Metadata)
                .Where(i => i.Quote.USD.MarketCap >= modelFilter.MarketCap && i.Quote.USD.Price >= modelFilter.Price);
            
            if (!string.IsNullOrEmpty(modelFilter.Name))
            {
                result = result.Where(i => i.Name.ToUpper().Contains(modelFilter.Name.ToUpper()));
            }

            totalCount = result.Count();

            result = result.Skip(modelFilter.PageSize * (modelFilter.PageNumber - 1))
                .Take(modelFilter.PageSize)
                .OrderByDescending(i => i.Quote.USD.Price);

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
