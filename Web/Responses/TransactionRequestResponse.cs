using CreditTransactionApi.Data;

namespace CreditTransactionApi.Web;

public class TransactionRequestResponse
{
    public string Code { get; set; } = null!;

    public TransactionRequestResponse(TransactionResultCode resultCode)
    {
        switch (resultCode)
        {
            case TransactionResultCode.APPROVED:
                Code = "00";
                break;
            case TransactionResultCode.INSUFFICIENT_FUNDS:
                Code = "51";
                break;
            case TransactionResultCode.REFUSED:
                Code = "07";
                break;
            default:
                Code = "07";
                break;
        }
    }
}