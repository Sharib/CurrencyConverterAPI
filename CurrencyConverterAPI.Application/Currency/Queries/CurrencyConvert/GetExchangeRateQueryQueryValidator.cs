using CurrencyConverterAPI.Application.Users.Queries.CurrencyConvert;
using CurrencyConverterAPI.Domain;
using CurrencyConverterAPI.Domain.Entities;
using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurrencyConverterAPI.Application.Users.Queries.GetUser;

public class GetCurrencyConvertQueryValidator : AbstractValidator<GetCurrencyConvertQuery>
{ 
    // Black Listed countries 
    string[] blackListed = { "TRY", "PLN", "THB", "MXN" };

    public GetCurrencyConvertQueryValidator()
    {
        RuleFor(u => u.amount)
            .NotEmpty()
            .WithMessage("Amount is Required");

        RuleFor(u => u.from)
            .NotEmpty()
            .WithMessage("Source currency is Required")
            .Must(HasMaterialPublishedElseWhereText)
             .WithMessage("TRY, PLN , THB or MXV currency are not allowed for conversion");

        RuleFor(u => u.to)
            .NotEmpty()
            .WithMessage("Target currency is Required")
            .Must(HasMaterialPublishedElseWhereText)
             .WithMessage("TRY, PLN , THB or MXV currency are not allowed for conversion");
    }

    private bool HasMaterialPublishedElseWhereText(CurrencyCode  code)
    {
        return blackListed.Contains(code.ToString());
    }
}