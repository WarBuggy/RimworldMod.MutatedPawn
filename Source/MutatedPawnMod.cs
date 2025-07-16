using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnMod : Mod
    {
        private readonly MutatedPawnSettings MutatedPawnSettings;
        private Vector2 scrollPosition = Vector2.zero; // Start at top-left
        private readonly string listSeparator = "|-+|";
        private bool initialized = false;

        public MutatedPawnMod(ModContentPack content)
           : base(content)
        {
            MutatedPawnSettings = base.GetSettings<MutatedPawnSettings>();
            LongEventHandler.QueueLongEvent(() =>
            {
                Initialize(); 
            }, "MutatedPawn_Initialize", false, null);
        }

        private void Initialize()
        {
            if (initialized) return;

            try
            {
                MutatedPawnSettings.allGenes = ConvertStringToGeneDefList(MutatedPawnSettings.allGenesInString);
                MutatedPawnSettings.disableViolenceGenes = ConvertStringToGeneDefList(MutatedPawnSettings.disableViolenceGenesInString);
                initialized = true;
            }
            catch (Exception e)
            {
                if (MutatedPawnSettings.debug)
                {
                    Log.Message($"MutatedPawn: Failed to process stored black/white list. Exception: {e}.");
                    Log.Message($"MutatedPawn: Failed to process stored black/white list. All genes are made available.");
                    MutatedPawnSettings.allGenes = DefDatabase<GeneDef>.AllDefs.ToList();
                    HandleArchiteGenes();
                    GetDisableViolenceGenes();
                    initialized = true;
                }
            }
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float viewHeight = 1200f;
            float scrollbarWidth = 16f;
            Rect viewRect = new Rect(0f, 0f, inRect.width - scrollbarWidth, viewHeight);
            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(viewRect);

            listingStandard.Label("Buggy_MP_Option_LoreFriendly".Translate());
            listingStandard.Gap();

            listingStandard.Label("Buggy_MP_Option_1stMutationChance".Translate());
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_MaxNumberOfMutation_1stGroup".Translate()}: {MutatedPawnSettings.maxMutatedGenesAllowed1stChance}", -1, "Buggy_MP_Option_MaxNumberOfMutation_1stGroup_Tooltip".Translate());
            MutatedPawnSettings.maxMutatedGenesAllowed1stChance = (int)listingStandard.Slider(MutatedPawnSettings.maxMutatedGenesAllowed1stChance, 0f, 10f);
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_ChanceToHaveAMutatedGene_1stGroup".Translate()}: {MutatedPawnSettings.percentChanceToHaveAMutatedGene1stChance}%", -1, "Buggy_MP_Option_ChanceToHaveAMutatedGene_1stGroup_Tooltip".Translate());
            MutatedPawnSettings.percentChanceToHaveAMutatedGene1stChance = (int)listingStandard.Slider(MutatedPawnSettings.percentChanceToHaveAMutatedGene1stChance, 0f, 100f);
            listingStandard.Gap();

            listingStandard.Label("Buggy_MP_Option_2ndMutationChance".Translate());
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_MaxNumberOfMutation_2ndGroup".Translate()}: {MutatedPawnSettings.maxMutatedGenesAllowed2ndChance}", -1, "Buggy_MP_Option_MaxNumberOfMutation_2ndGroup_Tooltip".Translate());
            MutatedPawnSettings.maxMutatedGenesAllowed2ndChance = (int)listingStandard.Slider(MutatedPawnSettings.maxMutatedGenesAllowed2ndChance, 0f, 10f);
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_ChanceToHaveAMutatedGene_2ndGroup".Translate()}: {MutatedPawnSettings.percentChanceToHaveAMutatedGene2ndChance}%", -1, "Buggy_MP_Option_ChanceToHaveAMutatedGene_2ndGroup_Tooltip".Translate());
            MutatedPawnSettings.percentChanceToHaveAMutatedGene2ndChance = (int)listingStandard.Slider(MutatedPawnSettings.percentChanceToHaveAMutatedGene2ndChance, 0f, 100f);
            listingStandard.Gap();

            listingStandard.Label("Buggy_MP_Option_3rdMutationChance".Translate());
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_MaxNumberOfMutation_3rdGroup".Translate()}: {MutatedPawnSettings.maxMutatedGenesAllowed3rdChance}", -1, "Buggy_MP_Option_MaxNumberOfMutation_3rdGroup_Tooltip".Translate());
            MutatedPawnSettings.maxMutatedGenesAllowed3rdChance = (int)listingStandard.Slider(MutatedPawnSettings.maxMutatedGenesAllowed3rdChance, 0f, 10f);
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_ChanceToHaveAMutatedGene_3rdGroup".Translate()}: {MutatedPawnSettings.percentChanceToHaveAMutatedGene3rdChance}%", -1, "Buggy_MP_Option_ChanceToHaveAMutatedGene_3rdGroup_Tooltip".Translate());
            MutatedPawnSettings.percentChanceToHaveAMutatedGene3rdChance = (int)listingStandard.Slider(MutatedPawnSettings.percentChanceToHaveAMutatedGene3rdChance, 0f, 100f);
            listingStandard.Gap();

            listingStandard.GapLine();
            listingStandard.Label("Buggy_MP_Option_NoneLoreFriendly".Translate());
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Buggy_MP_Option_AllowedMutatedXenoGene".Translate(), ref MutatedPawnSettings.allowedMutatedXenoGene, "Buggy_MP_Option_AllowedMutatedXenoGene_Tooltip".Translate());
            listingStandard.CheckboxLabeled("Buggy_MP_Option_AllowedMutatedArchiteGenes".Translate(), ref MutatedPawnSettings.allowedMutatedArchiteGenes, "Buggy_MP_Option_AllowedMutatedArchiteGenes_Tooltip".Translate());

            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_MinimumMetabolicEffForMutatedGen".Translate()}: {MutatedPawnSettings.minimumMetabolicEffAllowed}", -1, "Buggy_MP_Option_MinimumMetabolicEffForMutatedGen_Tooltip".Translate());
            MutatedPawnSettings.minimumMetabolicEffAllowed = (int)listingStandard.Slider(MutatedPawnSettings.minimumMetabolicEffAllowed, -50f, 50f);

            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_ChanceWithGrowingCarcinoma".Translate()}: {MutatedPawnSettings.chanceWithGrowingCarcinoma}%", -1, "Buggy_MP_Option_ChanceWithGrowingCarcinoma_Tooltip".Translate());
            MutatedPawnSettings.chanceWithGrowingCarcinoma = (int)listingStandard.Slider(MutatedPawnSettings.chanceWithGrowingCarcinoma, 0f, 100f);
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_TickPerGrowingCarcinomaCheck".Translate()}: {MutatedPawnSettings.tickPerGrowingCarcinomaCheck}", -1, "Buggy_MP_Option_TickPerGrowingCarcinomaCheck_Tooltip".Translate());
            MutatedPawnSettings.tickPerGrowingCarcinomaCheck = (int)listingStandard.Slider(MutatedPawnSettings.tickPerGrowingCarcinomaCheck, 1000f, 100000f);

            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_ChanceWithModerateToxicBuildup".Translate()}: {MutatedPawnSettings.chanceWithModerateToxicBuildup}%", -1, "Buggy_MP_Option_ChanceWithModerateToxicBuildup_Tooltip".Translate());
            MutatedPawnSettings.chanceWithModerateToxicBuildup = (int)listingStandard.Slider(MutatedPawnSettings.chanceWithModerateToxicBuildup, 0f, 100f);
            listingStandard.Label((TaggedString)$"{"Buggy_MP_Option_TickPerToxicBuildupCheck".Translate()}: {MutatedPawnSettings.tickPerToxicBuildupCheck}", -1, "Buggy_MP_Option_TickPerToxicBuildupCheck_Tooltip".Translate());
            MutatedPawnSettings.tickPerToxicBuildupCheck = (int)listingStandard.Slider(MutatedPawnSettings.tickPerToxicBuildupCheck, 1000f, 100000f);

            listingStandard.Label("Buggy_MP_Option_BlackList".Translate(), -1, "Buggy_MP_Option_BlackList_Tooltip".Translate());
            MutatedPawnSettings.blackListString = listingStandard.TextEntry(MutatedPawnSettings.blackListString, 3);
            listingStandard.Label("Buggy_MP_Option_WhiteList".Translate(), -1, "Buggy_MP_Option_WhiteList_Tooltip".Translate());
            MutatedPawnSettings.whiteListString = listingStandard.TextEntry(MutatedPawnSettings.whiteListString, 3);

            Rect buttonShowAllGeneRect = listingStandard.GetRect(25f);
            if (Widgets.ButtonText(buttonShowAllGeneRect, "Buggy_MP_Option_ShowAllGeneDefName".Translate()))
            {
                ShowAllAvailableGenes();
            }
            TooltipHandler.TipRegion(buttonShowAllGeneRect, "Buggy_MP_Option_ShowAllGeneDefName_Tooltip".Translate());
            listingStandard.Gap(10f);

            listingStandard.Gap();
            listingStandard.CheckboxLabeled("Buggy_MP_Option_Debug".Translate(), ref MutatedPawnSettings.debug);

            listingStandard.End();
            Widgets.EndScrollView();
        }

        public override string SettingsCategory()
        {
            return "Buggy_MP_Option_Mod_Name".Translate();
        }

        public override void WriteSettings()
        {
            GetAllGenesAfterWhiteListAndBlackList();
            HandleArchiteGenes();
            MutatedPawnSettings.allGenesInString = ConvertGeneDefListToString(MutatedPawnSettings.allGenes);
            GetDisableViolenceGenes();
            MutatedPawnSettings.disableViolenceGenesInString = ConvertGeneDefListToString(MutatedPawnSettings.disableViolenceGenes);
            base.WriteSettings();
        }

        private void GetDisableViolenceGenes()
        {
            MutatedPawnSettings.disableViolenceGenes = new List<GeneDef>();
            foreach (var gene in MutatedPawnSettings.allGenes)
            {
                if (gene.disabledWorkTags.HasFlag(WorkTags.Violent))
                {
                    MutatedPawnSettings.disableViolenceGenes.Add(gene);
                }
            }
        }

        private void HandleArchiteGenes()
        {
            if (!MutatedPawnSettings.allowedMutatedArchiteGenes)
            {
                MutatedPawnSettings.allGenes.RemoveAll(x => x.biostatArc > 0);
                if (MutatedPawnSettings.debug)
                {
                    Log.Message($"MutatedPawn: Archite genes are not allow. {MutatedPawnSettings.allGenes.Count} genes left.");
                }
            }
        }

        private void ShowAllAvailableGenes()
        {
            var list = DefDatabase<GeneDef>.AllDefs.ToList();
            Log.Message("MutatedPawn: All found genes:");
            List<string> allGeneList = new List<string>();
            foreach (var def in list)
            {
                allGeneList.Add($"{def.defName} - {def.LabelShortAdj}");
            }
            Log.Message(string.Join(", ", allGeneList));
        }

        private void GetAllGenesAfterWhiteListAndBlackList()
        {
            List<GeneDef> allAvailableGenes = DefDatabase<GeneDef>.AllDefs.ToList();
            if (MutatedPawnSettings.debug)
            {
                Log.Message($"MutatedPawn: {allAvailableGenes.Count} genes found.");
            }
            if (string.IsNullOrEmpty(MutatedPawnSettings.blackListString.Trim()))
            {
                MutatedPawnSettings.allGenes = allAvailableGenes.ToList();
                if (MutatedPawnSettings.debug)
                {
                    Log.Message($"MutatedPawn: No black list found. No genes are removed.");
                }
            }
            else
            {
                var blackList = MutatedPawnSettings.blackListString.Split(',').ToList();
                var blackListGenes = allAvailableGenes.Where(a =>
                    blackList.Any(b =>
                        (!string.IsNullOrEmpty(a.defName) && a.defName.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(a.LabelShortAdj) && a.LabelShortAdj.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0))).ToList();
                MutatedPawnSettings.allGenes = allAvailableGenes.Where(a => !blackListGenes.Contains(a)).ToList();
                if (MutatedPawnSettings.debug)
                {
                    if (blackListGenes.Count > 0)
                    {
                        Log.Message($"MutatedPawn: Black list found. {blackListGenes.Count} genes are removed the gene pool: {string.Join(", ", blackListGenes.Select(x => x.LabelShortAdj))}.");
                    }
                    else
                    {
                        Log.Message($"MutatedPawn: Black list found but contains no valid genes.");
                    }
                }
            }

            if (string.IsNullOrEmpty(MutatedPawnSettings.whiteListString.Trim()))
            {
                if (MutatedPawnSettings.debug)
                {
                    Log.Message($"MutatedPawn: No white list found. All remaining genes are allowed.");
                }
            }
            else
            {
                var whiteList = MutatedPawnSettings.whiteListString.Split(',').ToList();
                var whiteListGenes = allAvailableGenes.Where(a =>
                    whiteList.Any(b =>
                        (!string.IsNullOrEmpty(a.defName) && a.defName.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(a.LabelShortAdj) && a.LabelShortAdj.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0))).ToList();

                MutatedPawnSettings.allGenes.AddRange(whiteListGenes);
                if (whiteListGenes.Count < 1)
                {
                    if (MutatedPawnSettings.debug)
                    {
                        Log.Message($"MutatedPawn: White list found but contains no valid gene. All remaining genes are allowed.");
                    }
                }
                else if (MutatedPawnSettings.debug)
                {
                    Log.Message($"MutatedPawn: White list found. {whiteListGenes.Count} genes are added to the allowed pool: {string.Join(", ", whiteListGenes.Select(x => $"{x.defName} - {x.LabelShortAdj}"))}");
                }
            }

            if (MutatedPawnSettings.debug)
            {
                Log.Message($"MutatedPawn: Gene list after applied black and white list: {string.Join(", ", MutatedPawnSettings.allGenes.Select(x => $"{x.defName} - {x.LabelShortAdj}"))}.");
                Log.Message($"MutatedPawn: Number of genes after applied black and white list: {MutatedPawnSettings.allGenes.Count}.");
            }
        }

        private string ConvertGeneDefListToString(List<GeneDef> list)
        {
            List<string> defNameList = list.Select(g => g.defName).ToList();
            string result = string.Join(listSeparator, defNameList);
            if (MutatedPawnSettings.debug)
            {
                Log.Message($"MutatedPawn: {list.Count} gene def were converted to string.");
            }
            return result;
        }

        private List<GeneDef> ConvertStringToGeneDefList(string str)
        {
            List<string> list = str.Split(new string[] { listSeparator }, StringSplitOptions.None).ToList();
            List<GeneDef> result = list.Select(defName => DefDatabase<GeneDef>.GetNamedSilentFail(defName))
                  .Where(gene => gene != null).ToList();
            if (MutatedPawnSettings.debug)
            {
                Log.Message($"MutatedPawn: {result.Count} gene def were converted from string.");
            }
            return result;

        }
    }
}