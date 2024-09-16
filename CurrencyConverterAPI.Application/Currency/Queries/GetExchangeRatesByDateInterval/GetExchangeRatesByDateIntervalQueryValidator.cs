using FluentValidation;
using System.Text.RegularExpressions;

namespace CurrencyConverterAPI.Application.Currency.Queries.GetExchangeRatesByDateInterval
{
    public class GetExchangeRatesByDateIntervalQueryValidator : AbstractValidator<GetExchangeRatesByDateIntervalQuery>
    {
        public GetExchangeRatesByDateIntervalQueryValidator()
        {
            RuleFor(u => u.from)
                .NotEmpty()
                .WithMessage("Currency is Required");

            RuleFor(u => u.startDate)
               .NotEmpty()
               .WithMessage("Start Date is Required");

            RuleFor(u => u.endDate)
               .NotEmpty()
               .WithMessage("End Date is Required");
            
        }
    }
}
