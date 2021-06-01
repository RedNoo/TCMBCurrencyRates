using System.Collections.Generic;
using TCBMCurrencyRates.Models;

namespace TCMBCurrencyRates.Contracts
{
    public interface IExportCommand
    {
        public string Name { get; }

        public string Execute(List<Currency> currencyList);
    }
}
