public class TransactionRequestPayload
{
    public int account { get; set; }
    public decimal totalAmount { get; set; }
    public int mcc { get; set; }
    public string merchant { get; set; } = null!;
}