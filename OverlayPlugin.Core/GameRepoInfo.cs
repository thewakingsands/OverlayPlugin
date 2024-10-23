using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 7.0
        public static readonly Version version = new Version(7, 0);
        public static readonly int ActorMoveOpcode = 0x2BB;
        public static readonly int ActorSetPosOpcode = 0x232;
        public static readonly int BattleTalk2Opcode = 0x379;
        public static readonly int CountdownOpcode = 0x146;
        public static readonly int CountdownCancelOpcode = 0x0B2;
        public static readonly int CEDirectorOpcode = 0x2C9;
        public static readonly int MapEffectOpcode = 0x191;
        public static readonly int RSVDataOpcode = 0x16D;
        public static readonly int NpcYellOpcode = 0x0E4;
    }
}
