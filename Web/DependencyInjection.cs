using CreditTransactionApi.Services;

namespace CreditTransactionApi.Web;

public static class DependencyInjection
{
    public static void AddDepencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<TransactionRequestPayloadValidator>();
        builder.Services.AddSingleton<OutOfScopeGenerateAccountPayloadValidator>();

        builder.Services.AddScoped<OutOfScopeHelperService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
    }
}