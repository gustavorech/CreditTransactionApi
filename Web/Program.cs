using CreditTransactionApi.Data;
using CreditTransactionApi.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var connectionString = ConnectionString.GenerateFromEnvironment();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString, c => c.MigrationsAssembly("Data"))
);

builder.Services.AddSingleton<TransactionRequestPayloadValidator>();
builder.Services.AddSingleton<OutOfScopeGenerateAccountPayloadValidator>();

builder.Services.AddScoped<OutOfScopeHelperService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<AccountService>();

var app = builder.Build();

app.UseHealthChecks("/healthcheck");

app.Logger.LogInformation("Start: database migration");
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
context.Database.Migrate();
app.Logger.LogInformation("Finish: database migration");

app.MapPost("/out-of-scope/generate-account", async (OutOfScopeGenerateAccountPayload payload, OutOfScopeGenerateAccountPayloadValidator payloadValidator, OutOfScopeHelperService outOfScopeHelperService) =>
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
});

app.MapGet("/out-of-scope/account/{accountId}/balance", async (int accountId, OutOfScopeGenerateAccountPayloadValidator payloadValidator, OutOfScopeHelperService outOfScopeHelperService) =>
{
    var result = await outOfScopeHelperService.GetAccountBalance(accountId);

    return Results.Ok(result);
});

app.MapGet("/out-of-scope/account/{accountId}/requests", async (int accountId, OutOfScopeGenerateAccountPayloadValidator payloadValidator, OutOfScopeHelperService outOfScopeHelperService) =>
{
    var result = await outOfScopeHelperService.ListCompleteTransactionRequests(accountId);

    return Results.Ok(result);
});

app.MapPost("/transaction", async (TransactionRequestPayload payload, TransactionRequestPayloadValidator payloadValidator, DataContext context, TransactionService transactionService) =>
{
    var validationResult = payloadValidator.Validate(payload);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
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
    return Results.Ok($"Created /transaction/{payload.account}");

});

app.Run();