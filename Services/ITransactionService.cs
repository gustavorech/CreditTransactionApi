using CreditTransactionApi.Data;

namespace CreditTransactionApi.Services;

public interface ITransactionService
{
    Task InsertTransactionRequest(TransactionRequest transactionRequest);
    Task UpdateResultCode(TransactionRequest transactionRequest, TransactionResultCode resultCode);
    Task<TransactionResultCode> ExecuteTransactionRequest(TransactionRequest transactionRequest);
}