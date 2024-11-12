using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 7.05
        public static readonly Version version = new Version(7, 0);
        public static readonly int ActorMoveOpcode = 0x27B;
        public static readonly int ActorSetPosOpcode = 0x3AF;
        public static readonly int BattleTalk2Opcode = 0x126;
        public static readonly int CountdownOpcode = 0x08E;
        public static readonly int CountdownCancelOpcode = 0x18F;
        public static readonly int CEDirectorOpcode = 0x1B5;
        public static readonly int MapEffectOpcode = 0x1DA;
        public static readonly int RSVDataOpcode = 0x35A;
        public static readonly int NpcYellOpcode = 0x2D0;
    }
}
