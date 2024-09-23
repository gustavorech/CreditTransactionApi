using FluentValidation;

public class OutOfScopeGenerateAccountPayloadValidator : AbstractValidator<OutOfScopeGenerateAccountPayload>
{
    public OutOfScopeGenerateAccountPayloadValidator()
    {
        RuleFor(x => x.accountId)
            .GreaterThan(0);

        RuleFor(x => x.foodPartitionInitialAmount)
            .GreaterThanOrEqualTo(0)
            .PrecisionScale(6, 2, false);

        RuleFor(x => x.mealPartitionInitialAmount)
            .GreaterThanOrEqualTo(0)
            .PrecisionScale(6, 2, false);

        RuleFor(x => x.cashPartitionInitialAmount)
            .GreaterThanOrEqualTo(0)
            .PrecisionScale(6, 2, false);
    }
}