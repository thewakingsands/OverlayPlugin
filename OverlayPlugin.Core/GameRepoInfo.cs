using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.31
        public static readonly Version version = new Version(6, 3);
        public static readonly int CEDirectorOpcode = 0x3E6;
        public static readonly int MapEffectOpcode = 0x30E;
        public static readonly int RSVDataOpcode = 0x30F;
    }
}
