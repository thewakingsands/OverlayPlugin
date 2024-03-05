using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 6.5
        public static readonly Version version = new Version(6, 5);
        public static readonly int CEDirectorOpcode = 0x0D0;
        public static readonly int MapEffectOpcode = 0x3AB;
        public static readonly int RSVDataOpcode = 0x1F0;
        public static readonly int ActorMoveOpcode = 0x223;
        public static readonly int ActorSetPosOpcode = 0x318;
        public static readonly int BattleTalk2Opcode = 0x1B4;
        public static readonly int CountdownOpcode = 0x3A7;
        public static readonly int CountdownCancelOpcode = 0x22F;
        public static readonly int NpcYellOpcode = 0x242;
    }
}
