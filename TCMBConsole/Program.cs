using System;
using System.Collections.Generic;
using TCBMCurrencyRates;

namespace TCMBConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TbmbXml tbmbXml = new TbmbXml();

            var result = tbmbXml.FindAll("EUR,USD");
            foreach (var item in result)
            {
                Console.WriteLine("ISIM : " + item.Isim);
                Console.WriteLine("ForexBuying : " + item.ForexBuying);
            }
            Console.ReadKey();

            Dictionary<string, string> orderConfig = new Dictionary<string, string>();
            orderConfig.Add("Kod", "DESC");
            orderConfig.Add("Isim", "DESC");

            var currencyList = tbmbXml.Sort(orderConfig);
            foreach (var item in currencyList)
            {
                Console.Write("KOD : " + item.Kod);
                Console.Write(" İSİM : " + item.Isim);
                Console.Write(" SATIŞ FİYATI : " + item.ForexBuying);
                Console.WriteLine("");
            }
            Console.ReadKey();

            Console.WriteLine(tbmbXml.ExportToCsv(result));

            Console.ReadKey();

            Console.WriteLine(tbmbXml.ExportToXml(currencyList));

            Console.ReadKey();
        }
    }
}