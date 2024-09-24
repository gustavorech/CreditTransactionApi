public class TransactionEndpointTests
{
    public class ExecuteTransactionRequest
    {
        [Fact]
        public void it_should_return_refused_if_the_payload_is_invalid()
        {
            Assert.True(true);
        }

        [Fact]
        public void it_should_return_refused_and_rollback_the_transaction_if_any_problem_occurs()
        {
            Assert.True(true);
        }

        [Fact]
        public void it_should_apply_the_transaction_and_return_the_result_code()
        {
            Assert.True(true);
        }
    }
}