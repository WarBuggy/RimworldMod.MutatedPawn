using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnComp : ThingComp
    {
        public string MutationString = "";

        private static readonly float ToxicBuildUpModerateSeverity = 0.4f;

        public List<string> CreateMutationList()
        {
            return MutationString.Split(',').ToList();
        }

        public void AddMutation(string mutation)
        {
            if (string.IsNullOrEmpty(MutationString))
            {
                MutationString = mutation;
                return;
            }
            MutationString = $"{MutationString},{mutation}";
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref MutationString, "MutationString", "");
        }

        public override void CompTick()
        {
            Pawn pawn = (Pawn)parent;
            if (pawn.InCryptosleep)
            {
                return;
            }
            var debug = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().debug;
            var tickPerGrowingCarcinomaCheck = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().tickPerGrowingCarcinomaCheck * MutatedPawnSettings.TICK_PER_CARCINOMA_CHECK_MULTIPLIER;
            if (pawn.IsHashIntervalTick(tickPerGrowingCarcinomaCheck))
            {
                if (!Find.WorldPawns.Contains(pawn))
                {
                    HandleGrowingCarinoma(pawn, debug);
                }
            }
            var tickPerToxicBuildupCheck = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().tickPerToxicBuildupCheck * MutatedPawnSettings.TICK_PER_TOXIC_BUILDUP_CHECK_MULTIPLIER;
            if (pawn.IsHashIntervalTick(tickPerToxicBuildupCheck))
            {
                if (!Find.WorldPawns.Contains(pawn))
                {
                    HandlePoluttion(pawn, debug);
                }
            }
        }

        private void HandleGrowingCarinoma(Pawn pawn, bool debug)
        {
            if (!FoundAGrowingCarcinoma(pawn))
            {
                if (debug)
                {
                    Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} has no growing carcinoma.");
                }
                return;
            }
            var chanceWithGrowingCarcinoma = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().chanceWithGrowingCarcinoma;
            if (chanceWithGrowingCarcinoma <= 0)
            {
                return;
            }
            var chance = UnityEngine.Random.Range(0f, 100f);
            if (debug)
            {
                Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} has carcinoma mutation rolls {chance} and the mutate chance is {chanceWithGrowingCarcinoma}.");
            }
            if (chance > chanceWithGrowingCarcinoma)
            {
                return;
            }
            List<GeneDef> availableGenes = new List<GeneDef>(((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().allGenes);
            var pawnGenes = pawn.genes.GenesListForReading.Select(x => x.def).ToList();
            availableGenes.RemoveAll(x => pawnGenes.Contains(x));
            if (availableGenes.Count < 1)
            {
                Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} cannot have a carcinoma mutation due to the lack of available genes.");
                return;
            }
            float floatResult = UnityEngine.Random.Range(0, availableGenes.Count);
            var index = (int)Math.Floor(floatResult);
            var chosenGene = availableGenes[index];
            pawn.genes.AddGene(chosenGene, true);
            AddMutation(chosenGene.defName);
            SendLetter(pawn, "Buggy_MP_Option_LetterText_Source_GrowingCarcinoma".Translate(), chosenGene.LabelShortAdj);
        }

        private void HandlePoluttion(Pawn pawn, bool debug)
        {
            var toxicHeDiff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup);
            if (toxicHeDiff == null || toxicHeDiff.Severity < ToxicBuildUpModerateSeverity)
            {
                if (debug)
                {
                    Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} has no toxic build up of moderate severity or worse.");
                }
                return;
            }
            var chanceWithModerateToxicBuildup = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().chanceWithModerateToxicBuildup;
            var chance = UnityEngine.Random.Range(0f, 100f);
            if (debug)
            {
                Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} has toxic buildup mutation rolls {chance} and the mutate chance is {chanceWithModerateToxicBuildup}.");
            }
            if (chance > chanceWithModerateToxicBuildup)
            {
                return;
            }
            List<GeneDef> availableGenes = new List<GeneDef>(((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<MutatedPawnSettings>().allGenes);
            var pawnGenes = pawn.genes.GenesListForReading.Select(x => x.def).ToList();
            availableGenes.RemoveAll(x => pawnGenes.Contains(x));
            if (availableGenes.Count < 1)
            {
                if (debug)
                {
                    Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} cannot have a toxic buildup mutation due to the lack of available genes.");
                }
                return;
            }
            float floatResult = UnityEngine.Random.Range(0, availableGenes.Count);
            var index = (int)Math.Floor(floatResult);
            var chosenGene = availableGenes[index];
            pawn.genes.AddGene(chosenGene, true);
            AddMutation(chosenGene.defName);
            SendLetter(pawn, "Buggy_MP_Option_LetterText_Source_ToxicBuildup".Translate(), chosenGene.LabelShortAdj);
        }

        private bool FoundAGrowingCarcinoma(Pawn pawn)
        {
            var carcinomaHeDiffs = pawn.health.hediffSet.hediffs.Where(x => x.def == HediffDefOf.Carcinoma).ToList();
            if (!carcinomaHeDiffs.Any())
            {
                return false;
            }

            foreach (var item in carcinomaHeDiffs)
            {
                var compGrowthMode = item.TryGetComp<HediffComp_GrowthMode>();
                if (compGrowthMode == null || compGrowthMode.growthMode != HediffGrowthMode.Growing)
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        public void SendLetter(Pawn pawn, string mutationSource, string mutatedGene)
        {
            TaggedString letterLabel = "Buggy_MP_Option_LetterLabel".Translate(pawn.LabelShort);
            TaggedString letterMessage = "Buggy_MP_Option_LetterText".Translate(pawn.LabelShort, mutationSource, mutatedGene);
            LookTargets lookTarget = new LookTargets(pawn);
            ChoiceLetter letter = LetterMaker.MakeLetter(letterLabel, letterMessage, LetterDefOf.NegativeEvent, lookTarget, (Faction)null, (Quest)null, (List<ThingDef>)null);
            Find.LetterStack.ReceiveLetter((Letter)(object)letter, (string)null);
        }
    }
}
