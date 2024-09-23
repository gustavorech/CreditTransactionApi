using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;

[EntityTypeConfiguration(typeof(TransactionRequestConfiguration))]
public class TransactionRequest
{
    public Guid Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public int MerchantCategoryCode { get; set; }
    public string MerchantName { get; set; } = null!;
    public DateTimeOffset TransactionDate { get; set; }
    public TransactionResultCode? ResultCode { get; set; } = null!;

    public TransactionEntry? CreditTransaction { get; set; } = null!;
}