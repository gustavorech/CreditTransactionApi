using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;

[EntityTypeConfiguration(typeof(MerchantConfiguration))]
public class Merchant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int MerchantCategoryCode { get; set; }
    public MerchantCategory MerchantCategory { get; set; } = null!;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}