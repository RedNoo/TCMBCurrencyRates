using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using TCBMCurrencyRates.PluginEngine;
using TCMBCurrencyRates.Contracts;

namespace TCBMCurrencyRates.PluginStarter
{
    public static class PlugInStarter
    {
        public static List<IExportCommand> Start()
        {
            var setUp = new AppDomainSetup();
            setUp.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

            string basePath = "";
            if (AppDomain.CurrentDomain.BaseDirectory.EndsWith("\\"))
                basePath = AppDomain.CurrentDomain.BaseDirectory + "";
            else
                basePath = AppDomain.CurrentDomain.BaseDirectory + "\\"; // Unit Test HAck



            var permissionSet = new PermissionSet(PermissionState.None);
            permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, basePath));
            permissionSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, basePath + "PlugIns"));

            var plugInApplicationDomain = AppDomain.CreateDomain("Plug In App Domain", null, setUp, permissionSet);

            var plugInEngine = (PlugInEngine)plugInApplicationDomain.CreateInstanceAndUnwrap(typeof(PlugInEngine).Assembly.FullName, typeof(PlugInEngine).FullName);

            return plugInEngine.LoadPlugInCommands();
        }
    }
}