using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using TCBMCurrencyRates.Models;
using TCBMCurrencyRates.PluginStarter;
using TCMBCurrencyRates.Contracts;

namespace TCBMCurrencyRates
{
    public class TbmbXml
    {
        private readonly IMemoryCache _memCache;
        private const string cacheKey = "currencyrateskey";

        private TarihDate currencies;

        List<IExportCommand> cmdList;

        public TbmbXml()
        {
            _memCache = new MemoryCache(new MemoryCacheOptions());
            Load();
            cmdList = PlugInStarter.Start();
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TarihDate));

            using (var reader = XmlReader.Create("https://www.tcmb.gov.tr/kurlar/today.xml"))
            {
                currencies = (TarihDate)serializer.Deserialize(reader);
            }

            var cacheExpOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                Priority = CacheItemPriority.Normal
            };
            _memCache.Set(cacheKey, currencies, cacheExpOptions);
        }

        public List<Currency> Sort(Dictionary<string, string> propertyInfoList)
        {
            IOrderedEnumerable<Currency> list = null;

            foreach (var property in propertyInfoList)
            {
                var propertyInfo = typeof(Currency).GetProperty(property.Key);

                if (propertyInfoList.First().Key == property.Key)
                {
                    if (property.Value.Equals("ASC"))
                        list = currencies.Currency.OrderBy(x => propertyInfo.GetValue(x, null));
                    else
                        list = currencies.Currency.OrderByDescending(x => propertyInfo.GetValue(x, null));
                }
                else
                {
                    if (property.Value.Equals("ASC"))
                        list = list.ThenBy(x => propertyInfo.GetValue(x, null));
                    else
                        list = list.ThenByDescending(x => propertyInfo.GetValue(x, null));
                }
            }

            return list.ToList();
        }

        public List<Currency> Get()
        {
            if (IsNeedToRefreshData() || !_memCache.TryGetValue(cacheKey, out currencies))
                Load();
            return currencies.Currency;
        }

        public List<Currency> FindAll(string currenyList)
        {
            string[] filter = currenyList.Split(',');
            return currencies.Currency.Where(p => filter.Contains(p.Kod)).ToList();
        }

        public Currency Find(string currenyName)
        {
            return currencies.Currency.FirstOrDefault(p => p.Kod == currenyName);
        }

        public bool IsNeedToRefreshData()
        {
            string[] hm = DateTime.Now.ToString("HH:mm").Split(':');
            return ((DateTime.Today.CompareTo(Convert.ToDateTime(currencies.Tarih)) != 0 && (Convert.ToInt32(hm[0]) >= 15 && Convert.ToInt32(hm[1]) > 30))
                           && (DateTime.Now.Day != (int)DayOfWeek.Saturday)
                           && (DateTime.Now.Day != (int)DayOfWeek.Sunday) 
                           );
        }

        public string ExportToCsv(List<Currency> currencyList)
        {
            var cmd = cmdList.Find(x => x.Name == "CSV");
            return cmd.Execute(currencyList);
        }

        public string ExportToXml(List<Currency> currencyList)
        {
            var cmd = cmdList.Find(x => x.Name == "XML");
            return cmd.Execute(currencies.Currency);
        }
    }
}