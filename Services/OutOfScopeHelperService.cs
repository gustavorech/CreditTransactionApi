using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Services;

public class OutOfScopeHelperService
{
    private readonly DataContext dataContext;

    public OutOfScopeHelperService(DataContext context)
    {
        this.dataContext = context;
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