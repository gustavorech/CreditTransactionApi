using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Services;

// Non performatic / smelly / spaghetti / bad code follows
// Only to help see it working

public class OutOfScopeHelperService
{
    private readonly DataContext dataContext;

    public OutOfScopeHelperService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task GenerateAccountAndAdditionalDataIfNecessary(int accountId, decimal foodPartitionInitialAmount, decimal mealPartitionInitialAmount, decimal cashPartitionInitialAmount)
    {
        await GenerateAdditionalDataIfNecessary();

        var account = new Account
        {
            Id = accountId,
            AccountPartitions = new List<AccountPartition>
            {
                new AccountPartition
                {
                    AccountPartitionType = AccountPartitionType.FOOD,
                    Balance = foodPartitionInitialAmount
                },
                new AccountPartition
                {
                    AccountPartitionType = AccountPartitionType.MEAL,
                    Balance = mealPartitionInitialAmount
                },
                new AccountPartition
                {
                    AccountPartitionType = AccountPartitionType.CASH,
                    Balance = cashPartitionInitialAmount
                }
            }
        };

        dataContext.Accounts.Add(account);

        await dataContext.SaveChangesAsync();
    }

    public async Task<dynamic> GetAccountBalance(int accountId)
    {
        var account = await dataContext.Accounts
            .Include(c => c.AccountPartitions)
            .FirstAsync(c => c.Id == accountId);

        return account.AccountPartitions.Select(accountPartition => new
        {
            AccountPartitionType = accountPartition.AccountPartitionType.ToString(),
            accountPartition.Balance
        });
    }

    public async Task<dynamic> ListCompleteTransactionRequests(int accountId)
    {
        var transactionRequestModelsList = await dataContext.TransactionRequests
            .Where(c => c.AccountId == accountId)
            .OrderByDescending(c => c.TransactionDate)
            .ToListAsync();

        var transactionRequestList = transactionRequestModelsList.Select(transactionRequest =>
        {
            var transactionEntryModel = dataContext.TransactionEntries
                .Include(c => c.AccountPartition)
                .Include(c => c.Merchant)
                .ThenInclude(c => c.MerchantCategory)
                .FirstOrDefault(c => c.TransactionRequestId == transactionRequest.Id);

            dynamic transactionEntry = new { };
            dynamic merchant = new { };
            dynamic merchantCategory = new { };
            dynamic accountPartitionType = new { };

            if (transactionEntryModel is not null)
            {
                if (transactionEntryModel.Merchant is not null)
                {
                    merchant = new
                    {
                        transactionEntryModel.Merchant.Name,
                        transactionEntryModel.Merchant.MerchantCategoryCode,
                        AccountPartitionType = transactionEntryModel.Merchant.MerchantCategory.AccountPartitionType.ToString(),
                    };
                }

                transactionEntry = new
                {
                    transactionEntryModel.PreviousBalance,
                    transactionEntryModel.NewBalance,
                    AccountPartitionType = transactionEntryModel.AccountPartition.AccountPartitionType.ToString(),
                    Merchant = merchant,
                };
            }

            return new
            {
                transactionRequest.Amount,
                transactionRequest.MerchantName,
                transactionRequest.MerchantCategoryCode,
                transactionRequest.TransactionDate,
                Result = transactionRequest.ResultCode.ToString(),
                TransactionEntry = transactionEntry
            };
        });

        return new
        {
            CurrentBalance = await GetAccountBalance(accountId),
            TransactionRequests = transactionRequestList,
        };
    }

    private async Task GenerateAdditionalDataIfNecessary()
    {
        var shouldGenerateAdditionalData = !await dataContext.MerchantCategories.AnyAsync();
        if (!shouldGenerateAdditionalData)
        {
            return;
        }

        var merchantCategories = new List<MerchantCategory>
        {
            new MerchantCategory
            {
                Code = 5411,
                AccountPartitionType = AccountPartitionType.FOOD,
                Description = "Grocery Stores, Supermarkets"
            },
            new MerchantCategory
            {
                Code = 5412,
                AccountPartitionType = AccountPartitionType.FOOD,
                Description = "Candy, Nut, Confectionery Stores"
            },
            new MerchantCategory
            {
                Code = 5811,
                AccountPartitionType = AccountPartitionType.MEAL,
                Description = "Food Delivery"
            },
            new MerchantCategory
            {
                Code = 5812,
                AccountPartitionType = AccountPartitionType.MEAL,
                Description = "Restaurants"
            },
        };

        dataContext.MerchantCategories.AddRange(merchantCategories);

        var merchants = new List<Merchant>
        {
            new Merchant
            {
                Name = "MERCADO DO ZE               SAO PAULO BR",
                MerchantCategoryCode = 5411
            },
            new Merchant
            {
                Name = "PADARIA DO ZE               SAO PAULO BR",
                MerchantCategoryCode = 5811
            }
        };

        dataContext.Merchants.AddRange(merchants);
    }
}