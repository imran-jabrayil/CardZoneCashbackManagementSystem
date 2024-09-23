using CardZoneCashbackManagementSystem.Models.Requests;
using FluentValidation;

namespace CardZoneCashbackManagementSystem.Models.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(tx => tx.Amount)
            .GreaterThan(0);

        RuleFor(tx => tx.Type)
            .Must(type => type == "DEBIT" || type == "CREDIT");
    }
}