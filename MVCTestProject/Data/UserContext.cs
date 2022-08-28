using Microsoft.EntityFrameworkCore;
using MVCTestProject.DataModels;

namespace MVCTestProject.Data
{
    public class MVCTestProjectContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteItem> QuoteItems { get; set; }
        public DbSet<CryptocurrencyMetadata> CryptoMetadatas { get; set; }
        public MVCTestProjectContext(DbContextOptions<MVCTestProjectContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>().HasKey(c => c.CryprocurrencyId);
            modelBuilder.Entity<QuoteItem>().HasKey(c => c.QuoteId);
            modelBuilder.Entity<CryptocurrencyMetadata>().HasKey(c => c.CryptocurrencyServerId);

            modelBuilder.Entity<Cryptocurrency>()
                .HasOne(c => c.CryptocurrencyMetadata)
                .WithOne(m => m.Cryptocurrency)
                .HasForeignKey<CryptocurrencyMetadata>(cm => cm.CryptocurrencyServerId);

            modelBuilder.Entity<Cryptocurrency>()
                .HasOne(c => c.Quote)
                .WithOne(q => q.Cryptocurrency)
                .HasForeignKey<Quote>(qc => qc.CryprocurrencyId);

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.QuoteItem)
                .WithOne(u => u.Quote)
                .HasForeignKey<QuoteItem>(qu => qu.QuoteId);
        }
    }
}
