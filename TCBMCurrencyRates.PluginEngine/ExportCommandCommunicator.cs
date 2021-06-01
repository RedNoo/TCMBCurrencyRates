using System;
using System.Collections.Generic;
using TCBMCurrencyRates.Contracts;
using TCBMCurrencyRates.Models;

namespace TCBMCurrencyRates.PluginEngine
{
    public class ExportCommandCommunicator : MarshalByRefObject, IExportCommand
    {
        private readonly IExportCommand _RealCommand;

        public ExportCommandCommunicator(IExportCommand realCommand)
        {
            this._RealCommand = realCommand;
        }

        public string Name { get { return this._RealCommand.Name; } }

        public void Execute(List<Currency> currencyList)
        {
            this._RealCommand.Execute(currencyList);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}