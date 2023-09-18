using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.40
        public static readonly Version version = new Version(6, 4);
        public static readonly int CEDirectorOpcode = 0x34F;
        public static readonly int MapEffectOpcode = 0x11E;
        public static readonly int RSVDataOpcode = 0x16C;
    }
}
