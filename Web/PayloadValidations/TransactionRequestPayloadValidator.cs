using FluentValidation;

public class TransactionRequestPayloadValidator : AbstractValidator<TransactionRequestPayload>
{
    public TransactionRequestPayloadValidator()
    {
        RuleFor(x => x.account)
            .GreaterThan(0);

        RuleFor(x => x.totalAmount)
            .GreaterThan(0)
            .PrecisionScale(6, 2, false);

        RuleFor(x => x.mcc)
            .GreaterThan(0)
            .LessThanOrEqualTo(9999);

        RuleFor(x => x.merchant)
            .Length(40);
    }
}