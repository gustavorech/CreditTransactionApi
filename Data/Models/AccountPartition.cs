using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;

[EntityTypeConfiguration(typeof(AccountPartitionConfiguration))]
public class AccountPartition
{
    public Guid Id { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public AccountPartitionType AccountPartitionType { get; set; }
    public decimal Balance { get; set; }

    public ICollection<TransactionEntry> Transactions { get; set; } = new List<TransactionEntry>();
}