using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Services;

public interface ITransactionService
{
    Task InsertTransactionRequest(TransactionRequest transactionRequest);
    Task UpdateResultCode(TransactionRequest transactionRequest, TransactionResultCode resultCode);
    Task<TransactionResultCode> ExecuteTransactionRequest(TransactionRequest transactionRequest);
}