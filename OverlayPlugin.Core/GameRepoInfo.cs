using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.45a
        public static readonly Version version = new Version(6, 4);
        public static readonly int CEDirectorOpcode = 0x362;
        public static readonly int MapEffectOpcode = 0x378;
        public static readonly int RSVDataOpcode = 0x246;
    }
}
