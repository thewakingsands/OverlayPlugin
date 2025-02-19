﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FFXIVClientStructs.Global.FFXIV.Component.GUI;
using RainbowMage.OverlayPlugin.MemoryProcessors.FFXIVClientStructs;

namespace RainbowMage.OverlayPlugin.MemoryProcessors.AtkStage
{
    using AtkStage = global::FFXIVClientStructs.Global.FFXIV.Component.GUI.AtkStage;
    interface IAtkStageMemory62 : IAtkStageMemory { }

    class AtkStageMemory62 : AtkStageMemory, IAtkStageMemory62
    {
        private static long GetAtkStageSingletonAddress(TinyIoCContainer container)
        {
            var data = container.Resolve<Data>();
            return (long)data.GetClassInstanceAddress(DataNamespace.Global, "Component::GUI::AtkStage");
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
            AtkUnitList list = unitMgr.AllLoadedUnitsList.ToType();
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
            // These addon entries are confirmed from the FFXIVClientStructs repos
            { "_ActionCross", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionCross) },
            { "_ActionBar01", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar02", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionDoubleCrossL", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionDoubleCrossBase) },
            { "_ActionDoubleCrossR", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionDoubleCrossBase) },
            { "_CastBar", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonCastBar) },
            { "CharacterInspect", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonCharacterInspect) },
            { "ChatLogPanel_0", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonChatLogPanel) },
            { "ChatLogPanel_1", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonChatLogPanel) },
            { "ChatLogPanel_2", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonChatLogPanel) },
            { "ChatLogPanel_3", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonChatLogPanel) },
            { "ItemSearchResult", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonItemSearchResult) },
            { "Macro", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonMacro) },
            { "_PartyList", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonPartyList) },
            { "Teleport", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonTeleport) },

            // These addons are guessed based on patterns
            { "_ActionBar", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar03", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar04", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar05", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar06", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar07", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar08", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBar09", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },
            { "_ActionBarEx", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonActionBarX) },

            // These addons are guessed based on names matching up or based on github code search
            { "AOZNotebook", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonAOZNotebook) },
            { "ChocoboBreedTraining", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonChocoboBreedTraining) },
            { "ContentsFinder", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonContentsFinder) },
            { "ContentsFinderConfirm", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonContentsFinderConfirm) },
            { "ContextIconMenu", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonContextIconMenu) },
            { "ContextMenu", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonContextMenu) },
            { "_EnemyList", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonEnemyList) },
            { "_Exp", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonExp) },
            { "FateReward", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonFateReward) },
            { "FieldMarker", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonFieldMarker) },
            { "Gathering", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonGathering) },
            { "GatheringMasterpiece", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonGatheringMasterpiece) },
            { "GrandCompanySupplyReward", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonGrandCompanySupplyReward) },
            { "GuildLeve", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonGuildLeve) },
            { "_HudLayoutScreen", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonHudLayoutScreen) },
            { "_HudLayoutWindow", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonHudLayoutWindow) },
            { "ItemInspectionList", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonItemInspectionList) },
            { "ItemInspectionResult", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonItemInspectionResult) },
            { "JournalDetail", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonJournalDetail) },
            { "JournalResult", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonJournalResult) },
            { "LotteryDaily", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonLotteryDaily) },
            { "MaterializeDialog", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonMaterializeDialog) },
            { "MateriaRetrieveDialog", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonMateriaRetrieveDialog) },
            { "NamePlate", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonNamePlate) },
            { "NeedGreed", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonNeedGreed) },
            { "RaceChocoboResult", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRaceChocoboResult) },
            { "RecipeNote", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRecipeNote) },
            { "ReconstructionBox", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonReconstructionBox) },
            { "RelicNoteBook", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRelicNoteBook) },
            { "Repair", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRepair) },
            { "Request", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRequest) },
            { "RetainerList", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRetainerList) },
            { "RetainerSell", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRetainerSell) },
            { "RetainerTaskAsk", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRetainerTaskAsk) },
            { "RetainerTaskList", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRetainerTaskList) },
            { "RetainerTaskResult", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonRetainerTaskResult) },
            { "SalvageDialog", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSalvageDialog) },
            { "SalvageItemSelector", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSalvageItemSelector) },
            { "SatisfactionSupply", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSatisfactionSupply) },
            { "SelectIconString", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSelectIconString) },
            { "SelectOk", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSelectOk) },
            { "SelectString", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSelectString) },
            // Both of these seem to exist in memory somehow??
            { "SelectYesno", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSelectYesno) },
            { "_SelectYesNo", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSelectYesno) },
            { "ShopCardDialog", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonShopCardDialog) },
            { "Synthesis", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonSynthesis) },
            { "Talk", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonTalk) },
            { "WeeklyBingo", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonWeeklyBingo) },
            { "WeeklyPuzzle", typeof(global::FFXIVClientStructs.Global.FFXIV.Client.UI.AddonWeeklyPuzzle) },


            // These addons are known to exist but not mapped yet:
            // (double entries are intentional, they appear twice in the list in game memory)
            /**
            Achievement
            ActionDetail
            ActionMenu
            AddonContextMenuTitle
            AddonContextSub
            AdventureNoteBook
            AetherCurrent
            AreaMap
            ArmouryBoard
            Character
            CharacterStatus
            ChatLog
            CircleFinder
            CircleList
            ConfigKeybind
            ConfigSystem
            ContactList
            ContentsInfo
            ContentsNote
            ContentsReplaySetting
            CountDownSettingDialog
            CrossWorldLinkshell
            Currency
            CursorAddon
            CursorLocation
            Dawn
            DawnStory
            DragDropS
            Emote
            FadeBack
            FadeMiddle
            FateProgress
            Filter
            FilterSystem
            FishGuide2
            FishingNote
            FreeCompany
            FreeCompanyTopics
            GSInfoGeneral
            GatheringNote
            GoldSaucerInfo
            HousingMenu
            HowToList
            Hud
            HudLayout
            Inventory
            InventoryCrystalGrid
            InventoryCrystalGrid
            InventoryEventGrid0
            InventoryEventGrid0E
            InventoryEventGrid1
            InventoryEventGrid1E
            InventoryEventGrid2
            InventoryEventGrid2E
            InventoryExpansion
            InventoryGrid
            InventoryGrid0
            InventoryGrid0E
            InventoryGrid1
            InventoryGrid1E
            InventoryGrid2E
            InventoryGrid3E
            InventoryGridCrystal
            InventoryLarge
            ItemDetail
            JobHudWHM
            JobHudWHM0
            Journal
            JournalDetail
            JournalDetail
            LicenseViewer
            LinkShell
            LoadingTips
            LookingForGroup
            Marker
            McGuffin
            MinionNoteBook
            MiragePrismPrismItemDetail
            MonsterNote
            MountNoteBook
            MountSpeed
            NowLoading
            OperationGuide
            Orchestrion
            OrnamentNoteBook
            PlayGuide
            PvpProfile
            PvpProfileColosseum
            QuestRedoHud
            RecommendList
            ScenarioTree
            ScreenFrameSystem
            ScreenLog
            Social
            SocialList
            SupportDesk
            Tooltip
            VVDFinder
            WebLauncher
            _ActionContents
            _AllianceList1
            _AllianceList2
            _AreaText
            _AreaText
            _BagWidget
            _BattleTalk
            _ContentGauge
            _DTR
            _FlyText
            _FocusTargetInfo
            _Image
            _Image
            _Image3
            _Image3
            _LimitBreak
            _LocationTitle
            _LocationTitleShort
            _MainCommand
            _MainCross
            _MiniTalk
            _Money
            _NaviMap
            _Notification
            _ParameterWidget
            _PoisonText
            _PoisonText
            _PopUpText
            _ScreenInfoBack
            _ScreenInfoFront
            _ScreenText
            _Status
            _StatusCustom0
            _StatusCustom1
            _StatusCustom2
            _StatusCustom3
            _TargetCursor
            _TargetCursorGround
            _TargetInfo
            _TargetInfoBuffDebuff
            _TargetInfoCastBar
            _TargetInfoMainTarget
            _TextChain
            _TextClassChange
            _TextError
            _ToDoList
            _WideText
            _WideText
             */
        };

        public unsafe dynamic GetAddon(string name)
        {
            if (!AddonMap.ContainsKey(name) || !IsValid())
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
