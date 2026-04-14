using Newtonsoft.Json;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Extensions;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UCombat.Models;

namespace UCombat
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool AlsoCancelEffect { get; set; }
        public List<SurrenderConfig> SurrenderWeapons { get; set; }
        public List<ElastomerConfig> ElastomerWeapons { get; set; }
        public void LoadDefaults()
        {
            AlsoCancelEffect = false;
            SurrenderWeapons = new List<SurrenderConfig> { new SurrenderConfig(1023, true, 15)};
            ElastomerWeapons = new List<ElastomerConfig> { new ElastomerConfig(363, 10, 0.5f, 0.5f, 0.5f, true) };
        }
    }
}
