using RimWorld;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class Settings : ModSettings
    {
        public bool debug;
        public int maxMutatedGenesAllowed;
        public int percentChanceToHaveAMutatedGene;
        public bool allowedMutatedXenoGene;
        public bool allowedMutatedArchiteGenes;
        public bool allowedLowerThanMinMetabolicEff;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref debug, "debug", false, false);
            Scribe_Values.Look(ref maxMutatedGenesAllowed, "maxMutatedGenesAllowed", 4, false);
            Scribe_Values.Look(ref percentChanceToHaveAMutatedGene, "chanceToHaveAMutatedGene", 5, false);
            Scribe_Values.Look(ref allowedMutatedXenoGene, "allowedMutatedXenoGene", false, false);
            Scribe_Values.Look(ref allowedLowerThanMinMetabolicEff, "allowedLowerThanMinMetabolicEff", false, false);
            Scribe_Values.Look(ref allowedMutatedArchiteGenes, "allowedMutatedArchiteGenes", false, false);

            base.ExposeData();
        }
    }
}
