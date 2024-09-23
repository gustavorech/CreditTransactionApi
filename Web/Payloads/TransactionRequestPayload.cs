using CreditTransactionApi.Data;

public class TransactionRequestPayload
{
    public int account { get; set; }
    public decimal totalAmount { get; set; }
    public int mcc { get; set; }
    public string merchant { get; set; } = null!;

    public TransactionRequest GenerateModel()
    {
        return new()
        {
            AccountId = account,
            Amount = totalAmount,
            MerchantCategoryCode = mcc,
            MerchantName = merchant
        };
    }
}