using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FFXIVClientStructs.Global.FFXIV.Component.GUI;
using RainbowMage.OverlayPlugin.MemoryProcessors.AtkGui.FFXIVClientStructs;

namespace RainbowMage.OverlayPlugin.MemoryProcessors.AtkStage
{
    using AtkStage = global::FFXIVClientStructs.Global.FFXIV.Component.GUI.AtkStage;
    interface IAtkStageMemory62 : IAtkStageMemory { }

    class AtkStageMemory62 : AtkStageMemory, IAtkStageMemory62
    {
        private static int GetAtkStageSingletonAddress(TinyIoCContainer container)
        {
            var data = container.Resolve<FFXIVClientStructs.Data>();
            return (int)data.GetClassInstanceAddress(FFXIVClientStructs.DataNamespace.Global, "Component::GUI::AtkStage");
        }

        public AtkStageMemory62(TinyIoCContainer container) : base(container, GetAtkStageSingletonAddress(container)) { }

        public override Version GetVersion()
        {
            return new Version(6, 2);
        }

        public unsafe IntPtr GetAddonAddress(string name)
        {
            if (!IsValid())
            {
                return IntPtr.Zero;
            }

            // Our current address points to an instance of AtkStage
            // We need to traverse the object to AtkUnitManager, then check each pointer to see if it's the addon we're looking for

            if (atkStageInstanceAddress.ToInt64() == 0)
            {
                return IntPtr.Zero;
            }
            dynamic atkStage = ManagedType<AtkStage>.GetManagedTypeFromIntPtr(atkStageInstanceAddress, memory);
            dynamic raptureAtkUnitManager = atkStage.RaptureAtkUnitManager;
            dynamic unitMgr = raptureAtkUnitManager.AtkUnitManager;
            AtkUnitList list = unitMgr.AllLoadedUnitsList;
            long* entries = (long*)&list.AtkUnitEntries;

            for (var i = 0; i < list.Count; ++i)
            {
                var ptr = new IntPtr(entries[i]);
                dynamic atkUnit = ManagedType<AtkUnitBase>.GetManagedTypeFromIntPtr(ptr, memory);
                byte[] atkUnitName = atkUnit.Name;

                var atkUnitNameValue = FFXIVMemory.GetStringFromBytes(atkUnitName, 0, atkUnitName.Length);
                if (atkUnitNameValue.Equals(name))
                {
                    return atkUnit.ptr;
                }
            }

            return IntPtr.Zero;
        }

        private static Dictionary<string, Type> AddonMap = new Dictionary<string, Type>() {
            { "_PartyList", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonPartyList) },
        };

        public unsafe dynamic GetAddon(string name)
        {
            if (!AddonMap.ContainsKey(name) && !IsValid())
            {
                return null;
            }

            var ptr = GetAddonAddress(name);

            if (ptr != IntPtr.Zero)
            {
                return ManagedType<AtkStage>.GetDynamicManagedTypeFromIntPtr(ptr, memory, AddonMap[name]);
            }

            return null;
        }
    }
}
