using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin.MemoryProcessors
{
    public enum ObjectType : byte
    {
        Unknown = 0x00,
        PC = 0x01,
        Monster = 0x02,
        NPC = 0x03,
        Aetheryte = 0x05,
        Gathering = 0x06,
        Minion = 0x09
    }

    [Serializable]
    public class Combatant
    {
        public uint ID;
        public uint OwnerID;
        public ObjectType Type;
        public uint TargetID;

        public byte Job;
        public string Name;

        public int CurrentHP;
        public int MaxHP;

        public Single PosX;
        public Single PosY;
        public Single PosZ;
        public Single Rotation;

        public string Distance;
        public byte EffectiveDistance;

        public List<EffectEntry> Effects;

        public string DistanceString(Combatant target)
        {
            var distanceX = (float)Math.Abs(PosX - target.PosX);
            var distanceY = (float)Math.Abs(PosY - target.PosY);
            var distanceZ = (float)Math.Abs(PosZ - target.PosZ);
            var distance = (float)Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ));
            return distance.ToString("0.00");
        }
    }

    [Serializable]
    public class EnmityEntry
    {
        public uint ID;
        public uint OwnerID;
        public string Name;
        public uint Enmity;
        public bool isMe;
        public int HateRate;
        public byte Job;
    }

    [Serializable]
    public class AggroEntry
    {
        public uint ID;
        public string Name;
        public int HateRate;
        public int Order;
        public bool isCurrentTarget;

        public int CurrentHP;
        public int MaxHP;

        // Target of Enemy
        public EnmityEntry Target;

        // Effects
        public List<EffectEntry> Effects;
    }

    [Serializable]
    public class EffectEntry
    {
        public ushort BuffID;
        public ushort Stack;
        public float Timer;
        public uint ActorID;
        public bool isOwner;
    }

    public abstract class EnmityMemory
    {
        abstract public bool IsValid();
        abstract public Combatant GetTargetCombatant();
        abstract public Combatant GetSelfCombatant();
        abstract public Combatant GetFocusCombatant();
        abstract public Combatant GetHoverCombatant();
        abstract public List<Combatant> GetCombatantList();
        abstract public List<EnmityEntry> GetEnmityEntryList(List<Combatant> combatantList);
        abstract public unsafe List<AggroEntry> GetAggroList(List<Combatant> combatantList);
        abstract public bool GetInCombat();
    }
}
