using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TCBMCurrencyRates;

namespace TCBMCurrencyRatesTest
{
    [TestClass]
    public class TbmbXmlTest
    {
        [TestMethod]
        public void Get_All_Currency_Rates()
        {
            TbmbXml tbmbXml = new TbmbXml();
            int actual = tbmbXml.Get().Count;

            Assert.AreEqual(20, actual);
        }

        [TestMethod]
        public void Order_Currency_Rates()
        {
            TbmbXml tbmbXml = new TbmbXml();
            Dictionary<string, string> orderConfig = new Dictionary<string, string>();
            orderConfig.Add("Kod", "DESC");
            orderConfig.Add("Isim", "DESC");

            var currencyList = tbmbXml.Sort(orderConfig);

            Assert.AreEqual("XDR", currencyList.First().Kod);
        }
    }
}
