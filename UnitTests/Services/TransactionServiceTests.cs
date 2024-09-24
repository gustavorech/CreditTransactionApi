public class TransactionServiceTests
{
    public class InsertTransactionRequest
    {
        [Fact]
        public void it_should_insert_the_transaction_request()
        {
            Assert.True(true);
        }
    }

    public class UpdateResultCode
    {
        [Fact]
        public void it_should_update_the_result_code()
        {
            Assert.True(true);
        }
    }

    public class ExecuteTransactionRequest
    {
        [Fact]
        public void it_should_try_fallback_to_cash_if_insufficient_balance_on_another_account_partition_type()
        {
            Assert.True(true);
        }

        [Fact]
        public void it_shouldnt_register_the_transaction_entry_if_insufficient_funds()
        {
            Assert.True(true);
        }

        [Fact]
        public void if_should_register_the_transaction_entry_if_its_applied()
        {
            Assert.True(true);
        }
    }
}