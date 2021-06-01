using System;
using System.Collections.Generic;
using TCBMCurrencyRates.Models;
using TCMBCurrencyRates.Contracts;

namespace TCMBCurrencyRates.PluginEngine
{
    public class ExportCommandCommunicator : MarshalByRefObject, IExportCommand
    {
        private readonly IExportCommand _RealCommand;

        public ExportCommandCommunicator(IExportCommand realCommand)
        {
            this._RealCommand = realCommand;
        }

        public string Name { get { return this._RealCommand.Name; } }

        public string Execute(List<Currency> currencyList)
        {
            return this._RealCommand.Execute(currencyList);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}