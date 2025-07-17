using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnSettings : ModSettings
    {
        public int maxMutatedGenesAllowed1stChance = 1;
        public int percentChanceToHaveAMutatedGene1stChance = 33;

        public int maxMutatedGenesAllowed2ndChance = 2;
        public int percentChanceToHaveAMutatedGene2ndChance = 15;

        public int maxMutatedGenesAllowed3rdChance = 1;
        public int percentChanceToHaveAMutatedGene3rdChance = 5;

        public bool allowedMutatedXenoGene = false;
        public bool allowedMutatedArchiteGenes = false;
        public int minimumMetabolicEffAllowed = -5;

        public int chanceWithGrowingCarcinoma = 5;
        public static int DEFAULT_TICK_PER_CARCINOMA_CHECK = 50;
        public static int TICK_PER_CARCINOMA_CHECK_MULTIPLIER = 100;
        public int tickPerGrowingCarcinomaCheck = DEFAULT_TICK_PER_CARCINOMA_CHECK * TICK_PER_CARCINOMA_CHECK_MULTIPLIER;

        public int chanceWithModerateToxicBuildup = 5;
        public static int DEFAULT_TICK_PER_TOXIC_BUILDUP_CHECK = 50;
        public static int TICK_PER_TOXIC_BUILDUP_CHECK_MULTIPLIER = 100;
        public int tickPerToxicBuildupCheck = DEFAULT_TICK_PER_TOXIC_BUILDUP_CHECK * TICK_PER_TOXIC_BUILDUP_CHECK_MULTIPLIER;

        public string blackListString = "";
        public string whiteListString = "";

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
            Scribe_Values.Look(ref tickPerGrowingCarcinomaCheck, "tickPerGrowingCarcinomaCheck", DEFAULT_TICK_PER_CARCINOMA_CHECK * TICK_PER_CARCINOMA_CHECK_MULTIPLIER, false);

            Scribe_Values.Look(ref chanceWithModerateToxicBuildup, "chanceWithModerateToxicBuildup", 5, false);
            Scribe_Values.Look(ref tickPerToxicBuildupCheck, "tickPerToxicBuildupCheck", DEFAULT_TICK_PER_TOXIC_BUILDUP_CHECK * TICK_PER_TOXIC_BUILDUP_CHECK_MULTIPLIER, false);

            Scribe_Values.Look(ref blackListString, "blackListString", "", false);
            Scribe_Values.Look(ref whiteListString, "whiteListString", "", false);
            Scribe_Values.Look(ref allGenesInString, "allGenesInString", "", false);
            Scribe_Values.Look(ref disableViolenceGenesInString, "disableViolenceGenesInString", "", false);

            Scribe_Values.Look(ref debug, "debug", false, false);

            base.ExposeData();
        }
    }
}


