using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Services;

public class TransactionService
{
    private readonly DataContext context;
    private readonly AccountService accountService;

    public TransactionService(
        DataContext context,
        AccountService accountService)
    {
        this.context = context;
        this.accountService = accountService;
    }

    public async Task InsertTransactionRequest(TransactionRequest transactionRequest)
    {
        transactionRequest.TransactionDate = DateTimeOffset.UtcNow;
        context.TransactionRequests.Add(transactionRequest);
        await context.SaveChangesAsync();
    }

    public async Task UpdateResultCode(TransactionRequest transactionRequest, TransactionResultCode resultCode)
    {
        transactionRequest.ResultCode = resultCode;
        await context.SaveChangesAsync();
    }

    public async Task<TransactionResultCode> ExecuteTransactionRequest(TransactionRequest transactionRequest)
    {
        TransactionEntry transactionEntry = new()
        {
            TransactionRequest = transactionRequest
        };

        (AccountPartitionType partitionType, transactionEntry.Merchant) =
            await accountService.DecideAccountPartitionTypeForRequest(transactionRequest);

        var account = await context.Accounts
            .FirstAsync(c => c.Id == transactionRequest.AccountId);

        var transactionResultCode = await accountService.ApplyTransactionIfHasFunds(transactionEntry, account, partitionType);

        bool shouldTryFallback = transactionResultCode == TransactionResultCode.INSUFFICIENT_FUNDS
            && partitionType != AccountPartitionType.CASH;

        if (shouldTryFallback)
        {
            partitionType = AccountPartitionType.CASH;
            transactionResultCode = await accountService.ApplyTransactionIfHasFunds(transactionEntry, account, partitionType);
        }

        if (transactionResultCode == TransactionResultCode.APPROVED)
        {
            context.TransactionEntries.Add(transactionEntry);
        }

        await context.SaveChangesAsync();
        return transactionResultCode;
    }
}