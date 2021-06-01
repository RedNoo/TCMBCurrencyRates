using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TCBMCurrencyRates.Contracts;

namespace TCBMCurrencyRates.PluginEngine
{
    public class PlugInEngine : MarshalByRefObject
    {
        public List<IExportCommand> LoadPlugInCommands()
        {
            var commandList = new List<IExportCommand>();
            string basePath = AppDomain.CurrentDomain.BaseDirectory + "PlugIns";

            foreach (var filePath in Directory.GetFiles(basePath, "*.dll"))
            {
                var loadedAssembly = Assembly.LoadFile(filePath);
                var exportTypes = loadedAssembly.GetTypes().Where(t => typeof(IExportCommand).IsAssignableFrom(t));

                foreach (var exportType in exportTypes)
                {
                    var cmd = Activator.CreateInstance(exportType);
                    commandList.Add(new ExportCommandCommunicator(cmd as IExportCommand));
                }
            }

            return commandList;
        }
    }
}