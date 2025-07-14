using System.Collections.Generic;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnSettings : ModSettings
    {
        public int maxMutatedGenesAllowed1stChance;
        public int percentChanceToHaveAMutatedGene1stChance;

        public int maxMutatedGenesAllowed2ndChance;
        public int percentChanceToHaveAMutatedGene2ndChance;

        public int maxMutatedGenesAllowed3rdChance;
        public int percentChanceToHaveAMutatedGene3rdChance;

        public bool allowedMutatedXenoGene;
        public bool allowedMutatedArchiteGenes;
        public int minimumMetabolicEffAllowed;

        public int chanceWithGrowingCarcinoma;
        public int tickPerGrowingCarcinomaCheck;

        public int chanceWithModerateToxicBuildup;
        public int tickPerToxicBuildupCheck;

        public string blackListString;
        public string whiteListString;

        public bool debug;

        public string allGenesInString;
        public string disableViolenceGenesInString;

        public List<GeneDef> allGenes;
        public List<GeneDef> disableViolenceGenes;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref maxMutatedGenesAllowed1stChance, "maxMutatedGenesAllowed1stChance", 1, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene1stChance, "percentChanceToHaveAMutatedGene1stChance", 33, false);

            Scribe_Values.Look(ref maxMutatedGenesAllowed2ndChance, "maxMutatedGenesAllowed2ndChance", 2, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene2ndChance, "percentChanceToHaveAMutatedGene2ndChance", 15, false);

            Scribe_Values.Look(ref maxMutatedGenesAllowed3rdChance, "maxMutatedGenesAllowed3rdChance", 1, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene3rdChance, "percentChanceToHaveAMutatedGene3rdChance", 5, false);

            Scribe_Values.Look(ref allowedMutatedXenoGene, "allowedMutatedXenoGene", false, false);
            Scribe_Values.Look(ref allowedMutatedArchiteGenes, "allowedMutatedArchiteGenes", false, false);
            Scribe_Values.Look(ref minimumMetabolicEffAllowed, "minimumMetabolicEffAllowed", -5, false);

            Scribe_Values.Look(ref chanceWithGrowingCarcinoma, "chanceWithGrowingCarcinoma", 5, false);
            Scribe_Values.Look(ref tickPerGrowingCarcinomaCheck, "tickPerGrowingCarcinomaCheck", 5000, false);

            Scribe_Values.Look(ref chanceWithModerateToxicBuildup, "chanceWithModerateToxicBuildup", 5, false);
            Scribe_Values.Look(ref tickPerToxicBuildupCheck, "tickPerToxicBuildupCheck", 5000, false);

            Scribe_Values.Look(ref blackListString, "blackListString", "", false);
            Scribe_Values.Look(ref whiteListString, "whiteListString", "", false);
            Scribe_Values.Look(ref allGenesInString, "allGenesInString", "", false);
            Scribe_Values.Look(ref disableViolenceGenesInString, "disableViolenceGenesInString", "", false);

            Scribe_Values.Look(ref debug, "debug", false, false);

            base.ExposeData();
        }
    }
}


