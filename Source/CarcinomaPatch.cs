using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{

    [HarmonyPatch]
    [HarmonyPatch(typeof(HediffComp_GrowthMode), "CompPostTick")]
    public class CarcinomaPatch
    {
        private static readonly int CheckInternal = 5000;

        public static void Prefix(HediffComp_GrowthMode __instance, out CarcinomaPatchData __state)
        {
            __state = new CarcinomaPatchData(__instance.Pawn, ref __instance.growthMode);
        }

        public static void Postfix(ref float severityAdjustment, CarcinomaPatchData __state)
        {
            if (__state.GrowthMode != HediffGrowthMode.Growing)
            {
                return;
            }
            if (!__state.Pawn.IsHashIntervalTick(CheckInternal))
            {
                return;
            }
            var chanceWhenCarcinomaGrowing = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<Settings>().chanceWhenCarcinomaGrowing;
            var allowedMutatedArchiteGenes = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<Settings>().allowedMutatedArchiteGenes;
            if (chanceWhenCarcinomaGrowing <= 0)
            {
                return;
            }
            var debug = ((Mod)LoadedModManager.GetMod<MutatedPawnMod>()).GetSettings<Settings>().debug;
            var chance = UnityEngine.Random.Range(0f, 100f);
            if (debug)
            {
                Log.Message($"MutatedPawn: CarcinomaPatch rolls {chance} and the mutate chance is {chanceWhenCarcinomaGrowing}.");
            }
            if (chance > chanceWhenCarcinomaGrowing)
            {
                return;
            }
            List<GeneDef> availableGenes = DefDatabase<GeneDef>.AllDefs.ToList();
            if (!allowedMutatedArchiteGenes)
            {
                availableGenes.RemoveAll(x => x.biostatArc > 0);
            }
            var pawnGenes = __state.Pawn.genes.GenesListForReading.Select(x => x.def).ToList();
            availableGenes.RemoveAll(x => pawnGenes.Contains(x));
            float floatResult = UnityEngine.Random.Range(0, availableGenes.Count);
            var index = (int)Math.Floor(floatResult);
            var chosenGene = availableGenes[index];
            __state.Pawn.genes.AddGene(chosenGene, true);
            var mutatedPawnComp = __state.Pawn.GetComp<MutatedPawnComp>();
            if (mutatedPawnComp != null)
            {
                mutatedPawnComp.AddMutation(chosenGene.defName);
            }
            SendLetter(__state.Pawn, chosenGene.LabelShortAdj);
        }

        public readonly struct CarcinomaPatchData
        {
            public Pawn Pawn { get; }
            public HediffGrowthMode GrowthMode { get; }

            public CarcinomaPatchData(Pawn pawn, ref HediffGrowthMode growthMode)
            {
                Pawn = pawn;
                GrowthMode = growthMode;
            }
        }

        private static void SendLetter(Pawn pawn, string mutatedGene)
        {
            TaggedString letterLabel = "Buggy_MP_Option_LetterLabel".Translate(pawn.LabelShort);
            TaggedString letterMessage = "Buggy_MP_Option_LetterText".Translate(pawn.LabelShort, mutatedGene);
            LookTargets lookTarget = new LookTargets(pawn);
            ChoiceLetter letter = LetterMaker.MakeLetter(letterLabel, letterMessage, LetterDefOf.NegativeEvent, lookTarget, (Faction)null, (Quest)null, (List<ThingDef>)null);
            Find.LetterStack.ReceiveLetter((Letter)(object)letter, (string)null);
        }
    }
}
