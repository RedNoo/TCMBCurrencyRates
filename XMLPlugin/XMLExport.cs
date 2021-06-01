using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TCBMCurrencyRates.Models;
using TCMBCurrencyRates.Contracts;

namespace XMLPlugin
{
    public class CSVExport : IExportCommand
    {
        public string Name
        {
            get { return "XML"; }
        }

        public string Execute(List<Currency> currencyList)
        {
            return CreateXML(currencyList);
        }

        public static string CreateXML(List<Currency> data)
        {
            var xml = new XElement("Currencies",
                   from cur in data
                   select new XElement("Currency",
                                new XAttribute("Kod", cur.Kod),
                                  new XElement("CurrencyName", cur.CurrencyName),
                                  new XElement("ForexBuying", cur.ForexBuying),
                                  new XElement("ForexSelling", cur.ForexSelling)
                              ));

            return xml.ToString();
        }
    }
}