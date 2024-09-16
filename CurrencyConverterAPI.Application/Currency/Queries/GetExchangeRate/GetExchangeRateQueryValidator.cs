using CurrencyConverterAPI.Application.Users.Queries.GetExchangeRate;
using FluentValidation;

namespace CurrencyConverterAPI.Application.Users.Queries.GetUser;

public class GetExchangeRateQueryQueryValidator : AbstractValidator<GetExchangeRateQuery>
{
    public GetExchangeRateQueryQueryValidator()
    {
        RuleFor(u => u.currencyName )
            .NotEmpty()
            .WithMessage("Currency is Required");
    }
}