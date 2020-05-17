using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace RainbowMage.OverlayPlugin
{
    class NetworkParser
    {
        public static event EventHandler<OnlineStatusChangedArgs> OnOnlineStatusChanged;

        private static Type MessageType = null;
        private static int ActorControl142_Size = 0;
        private static int MessageType_Offset = 0;
        private static int ActorID_Offset = 0;
        private static int Category_Offset = 0;
        private static int Param1_Offset = 0;
        private static ushort ActorControl142_Opcode = 0;

        /**
         * We use reflection to calculate the field offsets since there's no public Machina DLL we could link
         * against. I want to avoid copying the relevant structures so reflection is the last option left.
        */

        public static void Init()
        {
            try
            {
                var mach = Assembly.Load("Machina.FFXIV");
                MessageType = mach.GetType("Machina.FFXIV.Headers.Server_MessageType");

                var ActorControl142 = mach.GetType("Machina.FFXIV.Headers.Server_ActorControl142");
                ActorControl142_Size = Marshal.SizeOf(ActorControl142);

                var headerOffset = GetOffset(ActorControl142, "MessageHeader");
                var msgHeader = ActorControl142.GetField("MessageHeader").FieldType;

                MessageType_Offset = headerOffset + GetOffset(msgHeader, "MessageType");
                ActorID_Offset = headerOffset + GetOffset(msgHeader, "ActorID");

                Category_Offset = GetOffset(ActorControl142, "category");
                Param1_Offset = GetOffset(ActorControl142, "param1");

                ActorControl142_Opcode = GetOpcode("ActorControl142");

#if DEBUG
                Registry.Resolve<ILogger>().Log(LogLevel.Debug, $"ActorControl142 = {ActorControl142_Opcode.ToString("x")}");
#endif

                FFXIVRepository.RegisterNetworkParser(Parse);
            } catch (System.IO.FileNotFoundException)
            {
                Registry.Resolve<ILogger>().Log(LogLevel.Error, Resources.NetworkParserNoFfxiv);
            } catch (Exception e)
            {
                Registry.Resolve<ILogger>().Log(LogLevel.Error, Resources.NetworkParserInitException, e);
            }
        }

        private static int GetOffset(Type type, string property)
        {
            int offset = 0;

            foreach (var prop in type.GetFields())
            {
                var customOffset = prop.GetCustomAttribute<FieldOffsetAttribute>();
                if (customOffset != null)
                {
                    offset = customOffset.Value;
                }

                if (prop.Name == property)
                {
                    break;
                }

                if (prop.FieldType.IsEnum)
                {
                    offset += Marshal.SizeOf(Enum.GetUnderlyingType(prop.FieldType));
                } else
                {
                    offset += Marshal.SizeOf(prop.FieldType);
                }
            }

            return offset;
        }

        private static object GetEnumValue(Type type, string name)
        {
            foreach (var value in type.GetEnumValues())
            {
                if (value.ToString() == name)
                    return Convert.ChangeType(value, Enum.GetUnderlyingType(type));
            }

            throw new Exception($"Enum value {name} not found in {type}!");
        }

        private static ushort GetOpcode(string name)
        {
            // FFXIV_ACT_Plugin 2.0.4.14 converted Server_MessageType from enum to struct. Deal with each type appropriately.
            if (MessageType.IsEnum)
            {
                return (ushort)GetEnumValue(MessageType, name);
            } else
            {
                var value = MessageType.GetField(name).GetValue(null);
                return (ushort)value.GetType().GetProperty("InternalValue").GetValue(value);
            }
        }

        public unsafe static void Parse(string id, long epoch, byte[] message)
        {
            if (message.Length >= ActorControl142_Size)
            {
                fixed (byte* buffer = message)
                {
                    /*
                    Server_ActorControl142* packet = (Server_ActorControl142*)buffer;

                    if (packet->MessageHeader.MessageType != Server_MessageType.ActorControl142) return;
                    if (packet->category != Server_ActorControlCategory.StatusUpdate) return;

                    OnOnlineStatusChanged?.Invoke(null, new OnlineStatusChangedArgs(packet->MessageHeader.ActorID, packet->param1));
                    */

                    if (*((ushort*)&buffer[MessageType_Offset]) != ActorControl142_Opcode) return;
                    if (*((ushort*)&buffer[Category_Offset]) != 0x1f8) return;

                    OnOnlineStatusChanged?.Invoke(null, new OnlineStatusChangedArgs(*(uint*)&buffer[ActorID_Offset], *(uint*)&buffer[Param1_Offset]));
                }
            }
        }
    }

    public class OnlineStatusChangedArgs : EventArgs
    {
        public uint Target { get; private set; }
        public uint Status { get; private set; }

        public OnlineStatusChangedArgs(uint target, uint status)
        {
            this.Target = target;
            this.Status = status;
        }
    }
}
