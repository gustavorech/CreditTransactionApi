using CreditTransactionApi.Data;

namespace CreditTransactionApi.Services;

public interface IAccountService
{
    Task<(AccountPartitionType accountPartitionType, Merchant? merchant)>
        DecideAccountPartitionTypeForRequest(TransactionRequest transactionRequest);

    Task<TransactionResultCode> ApplyTransactionIfHasFunds(
        TransactionEntry transactionEntry,
        Account account,
        AccountPartitionType accountPartitionType);
}