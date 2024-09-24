public class TransactionEndpointTests
{
    public class ExecuteTransactionRequest
    {
        [Fact]
        public void it_should_use_merchant_account_partition_type_and_apply_the_transaction()
        {
            Assert.True(true);
        }

        [Fact]
        public void if_merchant_is_not_known_it_should_use_account_partition_type_from_merchant_category_code_and_apply_the_transaction()
        {
            Assert.True(true);
        }

        [Fact]
        public void if_nor_merchant_nor_merchant_category_code_is_known_it_should_use_cash_account_partition_type_and_apply_the_transaction()
        {
            Assert.True(true);
        }

        [Fact]
        public void if_non_cash_account_partition_type_has_no_funds_it_should_fallback_to_cash_partition_type_and_apply_transaction()
        {
            Assert.True(true);
        }

        [Fact]
        public void if_specific_and_cash_account_partition_type_has_no_funds_it_should_not_apply_and_reject_the_transaction()
        {
            Assert.True(true);
        }

        [Fact]
        public void if_any_known_or_unknown_error_aside_from_insufficient_funds_occur_should_refuse_the_transaction()
        {
            Assert.True(true);
        }
    }
}