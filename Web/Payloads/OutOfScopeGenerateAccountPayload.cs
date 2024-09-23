using CreditTransactionApi.Data;

public class OutOfScopeGenerateAccountPayload
{
    public int accountId { get; set; }
    public decimal foodPartitionInitialAmount { get; set; }
    public decimal mealPartitionInitialAmount { get; set; }
    public decimal cashPartitionInitialAmount { get; set; }
}