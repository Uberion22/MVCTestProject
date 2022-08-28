using Microsoft.EntityFrameworkCore;
using MVCTestProject.DataModels;
using MVCTestProject.ViewModels.Cryptocurrency;

namespace MVCTestProject.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Cryptocurrency> CryptoModel { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public DbSet<QuoteItem> QuoteItem { get; set; }
        public DbSet<CryptocurrencyMetadata> CryptoMetadatas { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public UserContext(string connection)
        {
            connectionString = connection;
        }
        private string connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (connectionString != null)
            {
                var config = connectionString;
                optionsBuilder.UseSqlServer(config);
            }

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cryptocurrency>()
                .HasOne(c => c.Metadata)
                .WithOne(m => m.CryptoModel)
                .HasForeignKey<CryptocurrencyMetadata>(cm => cm.CryptoId);

            modelBuilder.Entity<Cryptocurrency>()
                .HasOne(c => c.Quote)
                .WithOne(q => q.Cryptocurrency)
                .HasForeignKey<Quote>(qc => qc.CryptocurrencyId);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.USD)
                .WithOne(u => u.Quote)
                .HasForeignKey<QuoteItem>(qu => qu.QuoteId);
        }
    }
}
