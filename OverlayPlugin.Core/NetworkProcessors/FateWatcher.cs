using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RainbowMage.OverlayPlugin.NetworkProcessors
{
    public class FateWatcher
    {
        private ILogger logger;
        private string region_;

        // Fate start
        // param1: fateID
        // param2: unknown
        //
        // Fate end
        // param1: fateID
        //
        // Fate update
        // param1: fateID
        // param2: progress (0-100)
        private struct OPCodes
        {
            public OPCodes(int add_, int remove_, int update_) { this.add = add_; this.remove = remove_; this.update = update_; }
            public int add;
            public int remove;
            public int update;
        };
        private OPCodes v5_1 = new OPCodes(
          0x74,
          0x79,
          0x9B
        );
        private OPCodes v5_2 = new OPCodes(
          0x935,
          0x936,
          0x93E
        );

        private Dictionary<string, OPCodes> opcodes = null;

        private Type MessageType = null;
        private Type ActorControl143 = null;
        private int ActorControl143_Size = 0;
        private int Header_Offset = 0;
        private Type msgHeader = null;
        private int MessageType_Offset = 0;
        private int Category_Offset = 0;
        private int Param1_Offset = 0;
        private int Param2_Offset = 0;
        private ushort ActorControl143_Opcode = 0;

        // fates<fateID, progress>
        private static ConcurrentDictionary<int, int> fates;

        public event EventHandler<FateChangedArgs> OnFateChanged;

        public FateWatcher(TinyIoCContainer container)
        {
            logger = container.Resolve<ILogger>();
            var ffxiv = container.Resolve<FFXIVRepository>();
            if (!ffxiv.IsFFXIVPluginPresent())
                return;

            var language = ffxiv.GetLocaleString();

            if (language == "ko")
                region_ = "ko";
            else if (language == "cn")
                region_ = "cn";
            else
                region_ = "intl";

            opcodes = new Dictionary<string, OPCodes>();
            opcodes.Add("ko", v5_1);
            opcodes.Add("cn", v5_2);
            opcodes.Add("intl", v5_2);

            fates = new ConcurrentDictionary<int, int>();

            var netHelper = container.Resolve<NetworkParser>();
            var mach = Assembly.Load("Machina.FFXIV");
            MessageType = mach.GetType("Machina.FFXIV.Headers.Server_MessageType");
            ActorControl143 = mach.GetType("Machina.FFXIV.Headers.Server_ActorControl143");
            ActorControl143_Size = Marshal.SizeOf(ActorControl143);

            Header_Offset = netHelper.GetOffset(ActorControl143, "MessageHeader");
            msgHeader = ActorControl143.GetField("MessageHeader").FieldType;

            MessageType_Offset = Header_Offset + netHelper.GetOffset(msgHeader, "MessageType");

            Category_Offset = netHelper.GetOffset(ActorControl143, "category");
            Param1_Offset = netHelper.GetOffset(ActorControl143, "param1");
            Param2_Offset = netHelper.GetOffset(ActorControl143, "param2");

            ActorControl143_Opcode = netHelper.GetOpcode("ActorControl143");
            ffxiv.RegisterNetworkParser(MessageReceived);
        }

        private unsafe void MessageReceived(string id, long epoch, byte[] message)
        {
            if (message.Length < ActorControl143_Size)
                return;

            fixed (byte* buffer = message)
            {
                if (*((ushort*)&buffer[MessageType_Offset]) == ActorControl143_Opcode)
                {
                    ProcessMessage(buffer, message);
                }
            }
        }

        public unsafe void ProcessMessage(byte* buffer, byte[] message)
        {
            int a = *((int*)&buffer[Category_Offset]);

            if (a == opcodes[region_].add)
            {
                AddFate(*(int*)&buffer[Param1_Offset]);
            }
            else if (a == opcodes[region_].remove)
            {
                RemoveFate(*(int*)&buffer[Param1_Offset]);
            }
            else if (a == opcodes[region_].update)
            {
                int param1 = *(int*)&buffer[Param1_Offset];
                int param2 = *(int*)&buffer[Param2_Offset];
                if (!fates.ContainsKey(param1))
                {
                    AddFate(param1);
                }
                if (fates[param1] != param2)
                {
                    UpdateFate(param1, param2);
                }
            }
        }

        private void AddFate(int fateID)
        {
            if (!fates.ContainsKey(fateID))
            {
                fates.TryAdd(fateID, 0);
                OnFateChanged(null, new FateChangedArgs("add", fateID, 0));
            }
        }

        private void RemoveFate(int fateID)
        {
            if (fates.ContainsKey(fateID))
            {
                OnFateChanged(null, new FateChangedArgs("remove", fateID, fates[fateID]));
                fates.TryRemove(fateID, out _);
            }
        }

        private void UpdateFate(int fateID, int progress)
        {
            fates.AddOrUpdate(fateID, progress, (int id, int prog) => progress);
            OnFateChanged(null, new FateChangedArgs("update", fateID, progress));
        }

        public void RemoveAndClearFates()
        {
            foreach (int fateID in fates.Keys)
            {
                OnFateChanged(null, new FateChangedArgs("remove", fateID, fates[fateID]));
            }
            fates.Clear();
        }

        public class FateChangedArgs : EventArgs
        {
            public string eventType { get; private set; }
            public int fateID { get; private set; }
            public int progress { get; private set; }

            public FateChangedArgs(string eventType, int fateID, int progress) : base()
            {
                this.eventType = eventType;
                this.fateID = fateID;
                this.progress = progress;
            }
        }
    }
}
