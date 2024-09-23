using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Data;

[EntityTypeConfiguration(typeof(TransactionAppliedConfiguration))]
public class TransactionEntry
{
    public Guid Id { get; set; }
    public Guid TransactionRequestId { get; set; }
    public TransactionRequest TransactionRequest { get; set; } = null!;
    public Guid AccountPartitionId { get; set; }
    public AccountPartition AccountPartition { get; set; } = null!;
    public Guid? MerchantId { get; set; }
    public Merchant? Merchant { get; set; } = null!;
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
}