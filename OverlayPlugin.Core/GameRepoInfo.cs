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
        public static readonly int CEDirectorOpcode = 0x07D;
        public static readonly int MapEffectOpcode = 0x191;
        public static readonly int RSVDataOpcode = 0x10E;
        public static readonly int ActorMoveOpcode = 0x2A0;
        public static readonly int ActorSetPosOpcode = 0x1D1;
        public static readonly int BattleTalk2Opcode = 0x231;
        public static readonly int CountdownOpcode = 0x2C8;
        public static readonly int CountdownCancelOpcode = 0x2FA;
        public static readonly int NpcYellOpcode = 0x3CB;
    }
}
