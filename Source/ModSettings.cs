using HugsLib.Settings;
using System.Collections.Generic;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public static class ModSettings
    {
        public static SettingHandle<int> maxMutatedGenesAllowed1stChance;
        public static SettingHandle<int> percentChanceToHaveAMutatedGene1stChance;
        public static SettingHandle<int> maxMutatedGenesAllowed2ndChance;
        public static SettingHandle<int> percentChanceToHaveAMutatedGene2ndChance;
        public static SettingHandle<int> maxMutatedGenesAllowed3rdChance;
        public static SettingHandle<int> percentChanceToHaveAMutatedGene3rdChance;
        public static SettingHandle<bool> allowedMutatedXenoGene;
        public static SettingHandle<bool> allowedMutatedArchiteGenes;
        public static SettingHandle<int> minimumMetabolicEffAllowed;
        public static SettingHandle<int> chanceWithGrowingCarcinoma;
        public static SettingHandle<int> chanceWithModerateToxicBuildup;
        public static SettingHandle<string> whiteListWildcardString;
        public static SettingHandle<string> whiteListString;
        public static SettingHandle<string> blackListWildcardString;
        public static SettingHandle<string> blackListString;
        public static SettingHandle<bool> showAllGeneDefNameOnLog;
        public static SettingHandle<bool> debug;
        public static List<GeneDef> allGenes;
        public static List<GeneDef> disableViolenceGenes;
    }
}