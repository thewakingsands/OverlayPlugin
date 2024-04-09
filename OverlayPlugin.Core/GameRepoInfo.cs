using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.51
        public static readonly Version version = new Version(6, 5);
        public static readonly int CEDirectorOpcode = 0x241;
        public static readonly int MapEffectOpcode = 0x0C7;
        public static readonly int RSVDataOpcode = 0x333;
        public static readonly int ActorMoveOpcode = 0x1DD;
        public static readonly int ActorSetPosOpcode = 0x1D5;
        public static readonly int BattleTalk2Opcode = 0x0DE;
        public static readonly int CountdownOpcode = 0x30B;
        public static readonly int CountdownCancelOpcode = 0x31D;
        public static readonly int NpcYellOpcode = 0x1F6;
    }
}
