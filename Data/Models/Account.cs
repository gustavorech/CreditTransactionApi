using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;

[EntityTypeConfiguration(typeof(AccountConfiguration))]
public class Account
{
    public int Id { get; set; }

    public ICollection<AccountPartition> AccountPartitions { get; set; } = new List<AccountPartition>();
}