using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class Settings : ModSettings
    {
        public bool debug;
        public int maxMutatedGenesAllowed1stChance;
        public int percentChanceToHaveAMutatedGene1stChance;
        public int maxMutatedGenesAllowed2ndChance;
        public int percentChanceToHaveAMutatedGene2ndChance;
        public int maxMutatedGenesAllowed3rdChance;
        public int percentChanceToHaveAMutatedGene3rdChance;
        public bool allowedMutatedXenoGene;
        public bool allowedMutatedArchiteGenes;
        public int minimumMetabolicEffAllowed;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref debug, "debug", false, false);
            Scribe_Values.Look(ref maxMutatedGenesAllowed1stChance, "maxMutatedGenesAllowed1stChance", 1, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene1stChance, "percentChanceToHaveAMutatedGene1stChance", 33, false);
            Scribe_Values.Look(ref maxMutatedGenesAllowed2ndChance, "maxMutatedGenesAllowed2ndChance", 2, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene2ndChance, "percentChanceToHaveAMutatedGene2ndChance", 15, false);
            Scribe_Values.Look(ref maxMutatedGenesAllowed3rdChance, "maxMutatedGenesAllowed3rdChance", 1, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene3rdChance, "percentChanceToHaveAMutatedGene3rdChance", 5, false);
            Scribe_Values.Look(ref allowedMutatedXenoGene, "allowedMutatedXenoGene", false, false);
            Scribe_Values.Look(ref minimumMetabolicEffAllowed, "minimumMetabolicEffAllowed", -5, false);
            Scribe_Values.Look(ref allowedMutatedArchiteGenes, "allowedMutatedArchiteGenes", false, false);

            base.ExposeData();
        }
    }
}
