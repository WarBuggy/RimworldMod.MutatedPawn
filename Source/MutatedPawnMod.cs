using Verse;
using HarmonyLib;
using System.Reflection;
using HugsLib;
using HugsLib.Settings;
using System.Linq;
using System.Collections.Generic;

namespace Buggy.RimworldMod.MutatedPawn
{
    [EarlyInit]
    public class MutatedPawnMod : ModBase
    {
        public override string ModIdentifier => "MutatedPawn";
        protected override bool HarmonyAutoPatch => false;

        public MutatedPawnMod()
        {
        }

        public override void EarlyInitialize()
        {
            base.EarlyInitialize();
            var harmony = new Harmony("Buggy.RimworldMod.MutatedPawn");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override void DefsLoaded()
        {
            base.DefsLoaded();
            ReadSettings();
        }

        public override void SettingsChanged()
        {
            base.SettingsChanged();
            ReadSettings();
        }

        private void ReadSettings()
        {
            ModSettings.maxMutatedGenesAllowed1stChance = Settings.GetHandle("maxMutatedGenesAllowed1stChance",
                "Buggy_MP_Option_MaxNumberOfMutation_1stGroup".Translate(),
                "Buggy_MP_Option_MaxNumberOfMutation_1stGroup_Tooltip".Translate(), 1,
                 Validators.IntRangeValidator(0, 10));
            ModSettings.percentChanceToHaveAMutatedGene1stChance = Settings.GetHandle("percentChanceToHaveAMutatedGene1stChance",
                "Buggy_MP_Option_ChanceToHaveAMutatedGene_1stGroup".Translate(),
                $"Buggy_MP_Option_ChanceToHaveAMutatedGene_1stGroup_Tooltip".Translate(), 33,
                Validators.IntRangeValidator(0, 100));

            ModSettings.maxMutatedGenesAllowed2ndChance = Settings.GetHandle("maxMutatedGenesAllowed2ndChance",
                "Buggy_MP_Option_MaxNumberOfMutation_2ndGroup".Translate(),
                "Buggy_MP_Option_MaxNumberOfMutation_2ndGroup_Tooltip".Translate(), 2,
                Validators.IntRangeValidator(0, 10));
            ModSettings.percentChanceToHaveAMutatedGene2ndChance = Settings.GetHandle("percentChanceToHaveAMutatedGene2ndChance",
                "Buggy_MP_Option_ChanceToHaveAMutatedGene_2ndGroup".Translate(),
                "Buggy_MP_Option_ChanceToHaveAMutatedGene_2ndGroup_Tooltip".Translate(), 15,
                Validators.IntRangeValidator(0, 100));

            ModSettings.maxMutatedGenesAllowed3rdChance = Settings.GetHandle("maxMutatedGenesAllowed3rdChance",
                "Buggy_MP_Option_MaxNumberOfMutation_3rdGroup".Translate(),
                "Buggy_MP_Option_MaxNumberOfMutation_3rdGroup_Tooltip".Translate(), 1,
                Validators.IntRangeValidator(0, 10));
            ModSettings.percentChanceToHaveAMutatedGene3rdChance = Settings.GetHandle("percentChanceToHaveAMutatedGene3rdChance",
                "Buggy_MP_Option_ChanceToHaveAMutatedGene_3rdGroup".Translate(),
                "Buggy_MP_Option_ChanceToHaveAMutatedGene_3rdGroup_Tooltip".Translate(), 5,
                Validators.IntRangeValidator(0, 100));

            ModSettings.allowedMutatedXenoGene = Settings.GetHandle("allowedMutatedXenoGene",
                "Buggy_MP_Option_AllowedMutatedXenoGene".Translate(),
                "Buggy_MP_Option_AllowedMutatedXenoGene_Tooltip".Translate(), false);

            ModSettings.allowedMutatedArchiteGenes = Settings.GetHandle("allowedMutatedArchiteGenes",
                "Buggy_MP_Option_AllowedMutatedArchiteGenes".Translate(),
                "Buggy_MP_Option_AllowedMutatedArchiteGenes_Tooltip".Translate(), false);

            ModSettings.minimumMetabolicEffAllowed = Settings.GetHandle("minimumMetabolicEffAllowed",
                "Buggy_MP_Option_MinimumMetabolicEffForMutatedGen".Translate(),
                "Buggy_MP_Option_MinimumMetabolicEffForMutatedGen_Tooltip".Translate(), -5,
                Validators.IntRangeValidator(-50, 50));

            ModSettings.chanceWithGrowingCarcinoma = Settings.GetHandle("chanceWithGrowingCarcinoma",
                "Buggy_MP_Option_ChanceWithGrowingCarcinoma".Translate(),
                "Buggy_MP_Option_ChanceWithGrowingCarcinoma_Tooltip".Translate(), 5,
                Validators.IntRangeValidator(0, 100));

            ModSettings.chanceWithModerateToxicBuildup = Settings.GetHandle("chanceWithModerateToxicBuildup",
                "Buggy_MP_Option_ChanceWithModerateToxicBuildup".Translate(),
                "Buggy_MP_Option_ChanceWithModerateToxicBuildup_Tooltip".Translate(), 5,
                Validators.IntRangeValidator(0, 100));

            ModSettings.whiteListWildcardString = Settings.GetHandle("whiteListWildcardString",
                "Buggy_MP_Option_WhiteListWildcard".Translate(),
                "Buggy_MP_Option_WhiteListWildcard_Tooltip".Translate(), "");

            ModSettings.whiteListString = Settings.GetHandle("whiteListString",
                "Buggy_MP_Option_WhiteList".Translate(),
                "Buggy_MP_Option_WhiteList_Tooltip".Translate(), "");

            ModSettings.blackListWildcardString = Settings.GetHandle("blackListWildcardString",
                "Buggy_MP_Option_BlackListWildcard".Translate(),
                "Buggy_MP_Option_BlackListWildcard_Tooltip".Translate(), "");

            ModSettings.blackListString = Settings.GetHandle("blackListString",
                "Buggy_MP_Option_BlackList".Translate(),
                "Buggy_MP_Option_BlackList_Tooltip".Translate(), "");

            ModSettings.showAllGeneDefNameOnLog = Settings.GetHandle("showAllGeneDefNameOnLog",
                "Buggy_MP_Option_ShowAllGeneDefName".Translate(),
                "Buggy_MP_Option_ShowAllGeneDefName_Tooltip".Translate(), false);

            ModSettings.debug = Settings.GetHandle("debug",
                "Buggy_MP_Option_Debug".Translate(), "", false);

            ShowAllAvailableGenes();
            GetAllGenesAfterWhiteListAndBlackList();
            HandleArchiteGenes();
            GetDisableViolenceGenes();
        }

        private void GetDisableViolenceGenes()
        {
            ModSettings.disableViolenceGenes = new List<GeneDef>();
            foreach (var gene in ModSettings.allGenes)
            {
                if (gene.disabledWorkTags.HasFlag(WorkTags.Violent))
                {
                    ModSettings.disableViolenceGenes.Add(gene);
                }
            }
        }

        private void HandleArchiteGenes()
        {
            if (!ModSettings.allowedMutatedArchiteGenes.Value)
            {
                ModSettings.allGenes.RemoveAll(x => x.biostatArc > 0);
                if (ModSettings.debug)
                {
                    Log.Message($"MutatedPawn: Archite genes are not allow. {ModSettings.allGenes.Count} genes left.");
                }
            }
        }

        private void ShowAllAvailableGenes()
        {
            if (ModSettings.showAllGeneDefNameOnLog.Value)
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
        }

        private void GetAllGenesAfterWhiteListAndBlackList()
        {
            List<GeneDef> allAvailableGenes = DefDatabase<GeneDef>.AllDefs.ToList();
            if (ModSettings.debug)
            {
                Log.Message($"MutatedPawn: {allAvailableGenes.Count} genes found.");
            }

            if (string.IsNullOrEmpty(ModSettings.whiteListWildcardString.Value.Trim()))
            {
                ModSettings.allGenes = allAvailableGenes;
                if (ModSettings.debug)
                {
                    Log.Message($"MutatedPawn: No white list wildcard found. All genes are allowed.");
                }
            }
            // TODO: continue while list wildcard and black list wildcard
            if (string.IsNullOrEmpty(ModSettings.whiteListString.Value.Trim()))
            {
                ModSettings.allGenes = allAvailableGenes;
                if (ModSettings.debug)
                {
                    Log.Message($"MutatedPawn: No white list found. All genes are allowed.");
                }
            }
            else
            {
                var whiteList = ModSettings.whiteListString.Value.Split(',').ToList();
                ModSettings.allGenes = allAvailableGenes.Where(x => whiteList.Contains(x.defName) || whiteList.Contains(x.LabelShortAdj)).ToList();
                if (ModSettings.allGenes.Count < 1)
                {
                    if (ModSettings.debug)
                    {
                        ModSettings.allGenes = allAvailableGenes;
                        Log.Message($"MutatedPawn: White list found but contains no valid gene. All genes are allowed.");
                    }
                }
                else if (ModSettings.debug)
                {
                    Log.Message($"MutatedPawn: White list found. Only {ModSettings.allGenes.Count} genes are allowed: {string.Join(", ", ModSettings.allGenes.Select(x => $"{x.defName} - {x.LabelShortAdj}"))}");
                }
            }
            if (string.IsNullOrEmpty(ModSettings.blackListString.Value.Trim()))
            {
                if (ModSettings.debug)
                {
                    Log.Message($"MutatedPawn: No black list found. No genes are removed.");
                }
            }
            else
            {
                var blackList = ModSettings.blackListString.Value.Split(',').ToList();
                var blackListGenes = allAvailableGenes.Where(x => blackList.Contains(x.defName) || blackList.Contains(x.LabelShortAdj)).ToList();
                ModSettings.allGenes.RemoveAll(x => blackListGenes.Contains(x));
                if (ModSettings.debug)
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
        }
    }
}