using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RainbowMage.OverlayPlugin.Updater;

namespace RainbowMage.OverlayPlugin.NetworkProcessors
{
    class OverlayPluginLogLines
    {
        public OverlayPluginLogLines(TinyIoCContainer container)
        {
            container.Register(new OverlayPluginLogLineConfig(container));
            container.Register(new LineMapEffect(container));
            container.Register(new LineFateControl(container));
            container.Register(new LineCEDirector(container));
        }
    }

    class OverlayPluginLogLineConfig
    {
        private Dictionary<string, OpcodeConfigEntry> opcodes = new Dictionary<string, OpcodeConfigEntry>();
        private ILogger logger;
        private FFXIVRepository repository;
        private PluginConfig config;
        private TinyIoCContainer container;

        public OverlayPluginLogLineConfig(TinyIoCContainer container)
        {
            this.container = container;
            logger = container.Resolve<ILogger>();
            repository = container.Resolve<FFXIVRepository>();
            config = container.Resolve<PluginConfig>();
            //CN 6.11
            opcodes.Add("MapEffect", new OpcodeConfigEntry()
            {
                opcode = 154,
                size = 11
            });
            opcodes.Add("CEDirector", new OpcodeConfigEntry()
            {
                opcode = 854,
                size = 16
            });
        }
        public IOpcodeConfigEntry this[string name]
        {
            get
            {
                return opcodes[name];
            }
        }
    }

    interface IOpcodeConfigEntry
    {
        uint opcode { get; }
        uint size { get; }
    }

    [JsonObject(NamingStrategyType = typeof(Newtonsoft.Json.Serialization.DefaultNamingStrategy))]
    class OpcodeConfigEntry : IOpcodeConfigEntry
    {
        public uint opcode { get; set; }
        public uint size { get; set; }
    }
}
