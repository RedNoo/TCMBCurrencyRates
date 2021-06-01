using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCBMCurrencyRates.Models;
using TCMBCurrencyRates.Contracts;

namespace CSVPlugin
{
    public class CSVExport : IExportCommand
    {
        public string Name
        {
            get { return "CSV"; }
        }

        public string Execute(List<Currency> currencyList)
        {
            return CreateCSV(currencyList);
        }

        public static string CreateCSV<T>(List<T> data, string seperator = ",")
        {
            var properties = typeof(T).GetProperties();
            var result = new StringBuilder();

            foreach (var row in data)
            {
                var values = properties.Select(p => p.GetValue(row, null));
                var line = string.Join(seperator, values);
                result.AppendLine(line);
            }

            return result.ToString();
        }
    }
}