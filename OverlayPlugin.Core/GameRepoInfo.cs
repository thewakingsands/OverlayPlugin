using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.58
        public static readonly Version version = new Version(6, 5);
        public static readonly int CEDirectorOpcode = 0x363;
        public static readonly int MapEffectOpcode = 0x8C;
        public static readonly int RSVDataOpcode = 0x329;
        public static readonly int ActorMoveOpcode = 0x13C;
        public static readonly int ActorSetPosOpcode = 0x31B;
        public static readonly int BattleTalk2Opcode = 0x32A;
        public static readonly int CountdownOpcode = 0x90;
        public static readonly int CountdownCancelOpcode = 0x206;
        public static readonly int NpcYellOpcode = 0x0FE;
    }
}
