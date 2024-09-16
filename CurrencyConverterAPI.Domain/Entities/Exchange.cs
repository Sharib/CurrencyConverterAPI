using System;
using System.Collections.Generic;

namespace CurrencyConverterAPI.Domain
{
    public class Exchange
    {
        public DateTime ReferenceDate { get; set; }
        public decimal ReferenceAmount { get; set; }
        public CurrencyCode ReferenceCurrency { get; set; }
        public IEnumerable<CurrencyRate> Rates { get; set; }
    }

    public class ExchangeRate
    {
        public string Name { get; set; }
        public CurrencyCode Code { get; set; }
    }
}
