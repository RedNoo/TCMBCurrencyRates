using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TCMBCurrencyRates.Contracts;
using TCMBCurrencyRates.PluginEngine;

namespace TCBMCurrencyRates.PluginEngine
{
    public class PlugInEngine : MarshalByRefObject
    {
        public List<IExportCommand> LoadPlugInCommands()
        {
            var commandList = new List<IExportCommand>();
            string basePath = "";
            if (AppDomain.CurrentDomain.BaseDirectory.EndsWith("\\"))
                basePath= AppDomain.CurrentDomain.BaseDirectory + "PlugIns";
            else
                basePath = AppDomain.CurrentDomain.BaseDirectory + "\\PlugIns"; // Unit Test HAck

            foreach (var filePath in Directory.GetFiles(basePath, "*.dll"))
            {
                var loadedAssembly = Assembly.LoadFile(filePath);

                try
                {
                    var exportTypes = loadedAssembly.GetTypes().Where(t => typeof(IExportCommand).IsAssignableFrom(t));

                    foreach (var exportType in exportTypes)
                    {
                        var cmd = Activator.CreateInstance(exportType);
                        commandList.Add(new ExportCommandCommunicator(cmd as IExportCommand));
                    }
                }
                catch
                {
                  
                }

            }

            return commandList;
        }
    }
}