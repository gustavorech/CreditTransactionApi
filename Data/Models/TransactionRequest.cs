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
    public string? ResultCode { get; set; } = null!;

    public Transaction? CreditTransaction { get; set; } = null!;
}