using MVCTestProject.DataModels;

namespace MVCTestProject.Services
{
    public interface IDatabaseManager<T>: IDisposable where T : class
    {
        public IQueryable<Cryptocurrency> GetCryptocurrencyList();

        public IQueryable<Cryptocurrency> GetCryptocurrencyByFilter(CryptocurrencyFilter modelFilter, out int totalCount);

        public void CreateOrUpdateCryptocurrency(IEnumerable<Cryptocurrency> cryptocurrencies);

        public Task<User> GetUserByEmailAndPasswordAsync(string email, string passWord);
        
        public Task<User> GetUserByEmailAsync(string email);

        public Task<int> AddUserAsync(User user);
    }
}
