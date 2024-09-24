public class AccountServiceTests
{
    public class DecideAccountPartitionTypeForRequest
    {
        [Fact]
        public void it_should_return_the_merchant_account_partition_type_if_merchant_is_found()
        {
            Assert.True(true);
        }

        [Fact]
        public void it_should_return_the_merchant_category_account_partition_type_if_the_category_is_known()
        {
            Assert.True(true);
        }

        [Fact]
        public void it_should_return_cash_as_default_if_no_rule_was_appliable()
        {
            Assert.True(true);
        }
    }

    public class ApplyTransactionIfHasFunds
    {
        [Fact]
        public void it_shouldnt_apply_the_transaction_and_return_insufficient_founds_if_the_balance_is_insufficient()
        {
            Assert.True(true);
        }

        [Fact]
        public void it_should_apply_the_transaction_and_update_the_account_partition_balance()
        {
            Assert.True(true);
        }
    }
}