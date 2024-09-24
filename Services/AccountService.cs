using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Services;

public class AccountService : IAccountService
{
    private readonly DataContext context;

    public AccountService(DataContext context)
    {
        this.context = context;
    }

    public async Task<(AccountPartitionType accountPartitionType, Merchant? merchant)>
        DecideAccountPartitionTypeForRequest(TransactionRequest transactionRequest)
    {
        var merchant = await context.Merchants
            .Include(c => c.MerchantCategory)
            .SingleOrDefaultAsync(c => c.Name == transactionRequest.MerchantName);

        bool shouldGetPartitionTypeFromMerchant = merchant is not null;
        if (shouldGetPartitionTypeFromMerchant)
        {
            return (merchant!.MerchantCategory.AccountPartitionType, merchant);
        }

        var merchantCategory = await context.MerchantCategories
            .SingleOrDefaultAsync(c => c.Code == transactionRequest.MerchantCategoryCode);

        if (merchantCategory is not null)
        {
            return (merchantCategory.AccountPartitionType, null);
        }

        AccountPartitionType defaultPartitionType = AccountPartitionType.CASH;
        return (defaultPartitionType, null);
    }

    public async Task<TransactionResultCode> ApplyTransactionIfHasFunds(
        TransactionEntry transactionEntry,
        Account account,
        AccountPartitionType accountPartitionType)
    {
        var accountPartition = await context.AccountPartitions
            .SingleAsync(c => c.AccountId == account.Id && c.AccountPartitionType == accountPartitionType);

        transactionEntry.AccountPartition = accountPartition;

        if (accountPartition.Balance < transactionEntry.TransactionRequest.Amount)
        {
            return TransactionResultCode.INSUFFICIENT_FUNDS;
        }

        transactionEntry.PreviousBalance = accountPartition.Balance;
        transactionEntry.NewBalance = accountPartition.Balance - transactionEntry.TransactionRequest.Amount;
        accountPartition.Balance = transactionEntry.NewBalance;

        await context.SaveChangesAsync();

        return TransactionResultCode.APPROVED;
    }
}