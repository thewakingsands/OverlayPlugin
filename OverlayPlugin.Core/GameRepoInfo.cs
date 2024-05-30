using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.55
        public static readonly Version version = new Version(6, 5);
        public static readonly int CEDirectorOpcode = 0x2FF;
        public static readonly int MapEffectOpcode = 0x12D;
        public static readonly int RSVDataOpcode = 0x126;
        public static readonly int ActorMoveOpcode = 0x3DD;
        public static readonly int ActorSetPosOpcode = 0x1C3;
        public static readonly int BattleTalk2Opcode = 0x091;
        public static readonly int CountdownOpcode = 0x149;
        public static readonly int CountdownCancelOpcode = 0x211;
        public static readonly int NpcYellOpcode = 0x0E9;
    }
}
