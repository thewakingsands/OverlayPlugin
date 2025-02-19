using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class GameRepoInfo
    {
        //CN 7.1
        public static readonly Version version = new Version(7, 0);
        public static readonly int ActorMoveOpcode = 0x123;
        public static readonly int ActorSetPosOpcode = 0x2BC;
        public static readonly int BattleTalk2Opcode = 0x1C4;
        public static readonly int CountdownOpcode = 0x233;
        public static readonly int CountdownCancelOpcode = 0x33C;
        public static readonly int CEDirectorOpcode = 0x36A;
        public static readonly int MapEffectOpcode = 0x175;
        public static readonly int RSVDataOpcode = 0x3D6;
        public static readonly int NpcYellOpcode = 0x3CD;
    }
}
