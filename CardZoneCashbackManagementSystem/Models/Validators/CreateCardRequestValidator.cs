using CardZoneCashbackManagementSystem.Models.Requests;
using CardZoneCashbackManagementSystem.Utils;
using FluentValidation;

namespace CardZoneCashbackManagementSystem.Models.Validators;

public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
{
    public CreateCardRequestValidator()
    {
        RuleFor(ccr => ccr.Pan)
            .Length(16)
            .Must(pan => pan.All(char.IsDigit))
            .Must(CardUtils.ValidateLuhn);

        RuleFor(ccr => ccr.CustomerId)
            .MaximumLength(100);
    }
}