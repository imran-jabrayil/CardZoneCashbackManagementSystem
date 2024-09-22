using FluentValidation;

using CardZoneCashbackManagementSystem.Models.Requests;


namespace CardZoneCashbackManagementSystem.Models.Validators;

public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
{
    public CreateCardRequestValidator()
    {
        base.RuleFor(ccr => ccr.Pan)
            .Length(16)
            .Must(pan => pan.All(char.IsDigit));

        base.RuleFor(ccr => ccr.CustomerId)
            .MaximumLength(100);
    }
}