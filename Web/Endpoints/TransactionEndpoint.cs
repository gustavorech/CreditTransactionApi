using CreditTransactionApi.Data;
using CreditTransactionApi.Services;

namespace CreditTransactionApi.Web;

public static class TransactionEndpoint
{
    public static void UseTransactionEndpoints(this WebApplication app)
    {
        app.MapPost("/transaction", TransactionEndpoint.ExecuteTransactionRequest);
    }

    public static async Task<IResult> ExecuteTransactionRequest(
        TransactionRequestPayload payload,
        TransactionRequestPayloadValidator payloadValidator,
        DataContext context,
        ITransactionService transactionService)
    {
        var validationResult = payloadValidator.Validate(payload);
        if (!validationResult.IsValid)
        {
            return Results.Ok(new TransactionRequestResponse(TransactionResultCode.REFUSED));
        }

        TransactionRequest transactionRequest = payload.GenerateModel();
        await transactionService.InsertTransactionRequest(transactionRequest);

        TransactionResultCode resultCode = default;
        var transaction = context.Database.BeginTransaction();
        try
        {
            resultCode = await transactionService.ExecuteTransactionRequest(transactionRequest);
            transaction.Commit();
        }
        catch (Exception)
        {
            resultCode = TransactionResultCode.REFUSED;
            transaction.Rollback();
        }

        await transactionService.UpdateResultCode(transactionRequest, resultCode);
        return Results.Ok(new TransactionRequestResponse(resultCode));
    }
}