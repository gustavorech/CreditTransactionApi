using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;

[EntityTypeConfiguration(typeof(MerchantCategoryConfiguration))]
public class MerchantCategory
{
    public int Code { get; set; }
    public string Description { get; set; } = null!;

    public ICollection<Merchant> Merchants { get; set; } = new List<Merchant>();
}