using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountPartition> AccountPartitions { get; set; }
    public DbSet<MerchantCategory> MerchantCategories { get; set; }
    public DbSet<TransactionRequest> TransactionRequests { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}