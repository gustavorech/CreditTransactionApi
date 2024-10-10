using CreditTransactionApi.Services;

namespace CreditTransactionApi.Web;

public static class OutOfScopeEndpoint
{
    public static void UseOutOfScopeEndpoints(this WebApplication app)
    {
        var outOfScopeEndpoints = app.MapGroup("/out-of-scope");

        outOfScopeEndpoints.MapPost("generate-account", OutOfScopeEndpoint.GenerateAccountAndAdditionalDataIfNecessary);
        outOfScopeEndpoints.MapGet("account/{accountId}/balance", OutOfScopeEndpoint.GetAccountBalance);
        outOfScopeEndpoints.MapGet("account/{accountId}/requests", OutOfScopeEndpoint.ListCompleteTransactionRequests);
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