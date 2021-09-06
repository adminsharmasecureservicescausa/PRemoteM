﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PRM.Core.Protocol;
using PRM.Core.Protocol.Runner;

namespace PRM.Core.Service
{
    public class ProtocolConfigurationService
    {
        public Dictionary<string, ProtocolConfig> ProtocolConfigs { get; set; } = new Dictionary<string, ProtocolConfig>();
        public Dictionary<string, Dictionary<string, ProtocolRunner>> ProtocolRunners { get; set; } = new Dictionary<string, Dictionary<string, ProtocolRunner>>();

        public readonly string ProtocolFolderName;
        public ProtocolConfigurationService()
        {
            var appDateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ConfigurationService.AppName);
            ProtocolFolderName = Path.Combine(appDateFolder, "Protocols");
            if (Directory.Exists(ProtocolFolderName) == false)
                Directory.CreateDirectory(ProtocolFolderName);
        }

        public void Load()
        {   
            //// reflect all the child class
            //lock (this)
            //{
            //    if (_baseList.Count == 0)
            //    {
            //        var assembly = typeof(ProtocolServerBase).Assembly;
            //        var types = assembly.GetTypes();
            //        _baseList = types.Where(x => x.IsSubclassOf(typeof(ProtocolRunner)) && !x.IsAbstract)
            //            .Select(type => (ProtocolRunner)Activator.CreateInstance(type)).ToList();
            //    }
            //}
            ProtocolRunners.Clear();
            ProtocolConfigs.Clear();
            var di = new DirectoryInfo(ProtocolFolderName);
            foreach (var directoryInfo in di.GetDirectories())
            {
                var pn = directoryInfo.Name;
                var cfgPath = Path.Combine(directoryInfo.FullName, $"{pn}.json");
                if (File.Exists(cfgPath))
                {
                    var c = JsonConvert.DeserializeObject<ProtocolConfig>(File.ReadAllText(cfgPath, Encoding.UTF8));
                    if (c != null)
                        ProtocolConfigs.Add(pn, c);
                }

                ProtocolRunners.Add(pn,new Dictionary<string, ProtocolRunner>());

                foreach (var fi in directoryInfo.GetFiles("*.json"))
                {
                    
                }


                if (ProtocolConfigs.ContainsKey(pn) == false)
                {
                    ProtocolConfigs.Add(pn, new ProtocolConfig(pn));
                }
            }
        }
    }
}
