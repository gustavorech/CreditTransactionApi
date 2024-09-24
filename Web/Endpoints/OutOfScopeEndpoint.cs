using CreditTransactionApi.Services;

namespace CreditTransactionApi.Web;

public static class OutOfScopeEndpoint
{
    public static void UseOutOfScopeEndpoints(this WebApplication app)
    {
        app.MapPost("/out-of-scope/generate-account", OutOfScopeEndpoint.GenerateAccountAndAdditionalDataIfNecessary);
        app.MapGet("/out-of-scope/account/{accountId}/balance", OutOfScopeEndpoint.GetAccountBalance);
        app.MapGet("/out-of-scope/account/{accountId}/requests", OutOfScopeEndpoint.ListCompleteTransactionRequests);
    }

    public static async Task<IResult> GenerateAccountAndAdditionalDataIfNecessary(
        OutOfScopeGenerateAccountPayload payload,
        OutOfScopeGenerateAccountPayloadValidator payloadValidator,
        OutOfScopeHelperService outOfScopeHelperService)
    {
        var validationResult = payloadValidator.Validate(payload);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        await outOfScopeHelperService.GenerateAccountAndAdditionalDataIfNecessary(
            payload.accountId,
            payload.foodPartitionInitialAmount,
            payload.mealPartitionInitialAmount,
            payload.cashPartitionInitialAmount
        );

        return Results.Ok($"Created /out-of-scope/generate-account/{payload.accountId}");
    }

    public static async Task<IResult> GetAccountBalance(
        int accountId,
        OutOfScopeGenerateAccountPayloadValidator payloadValidator,
        OutOfScopeHelperService outOfScopeHelperService)
    {
        var result = await outOfScopeHelperService.GetAccountBalance(accountId);

        return Results.Ok(result);
    }

    public static async Task<IResult> ListCompleteTransactionRequests(
        int accountId,
        OutOfScopeGenerateAccountPayloadValidator payloadValidator,
        OutOfScopeHelperService outOfScopeHelperService)
    {
        var result = await outOfScopeHelperService.ListCompleteTransactionRequests(accountId);

        return Results.Ok(result);
    }
}