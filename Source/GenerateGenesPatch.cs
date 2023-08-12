using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(PawnGenerator), "GenerateGenes")]
    public class GenerateGenesPatch
    {
        public static void Postfix(Pawn pawn, XenotypeDef xenotype, PawnGenerationRequest request)
        {
            var chanceDictionary = new List<(int, int)>()
            {
                (ModSettings.maxMutatedGenesAllowed1stChance.Value, ModSettings.percentChanceToHaveAMutatedGene1stChance.Value),
                (ModSettings.maxMutatedGenesAllowed2ndChance.Value, ModSettings.percentChanceToHaveAMutatedGene2ndChance.Value),
                (ModSettings.maxMutatedGenesAllowed3rdChance.Value, ModSettings.percentChanceToHaveAMutatedGene3rdChance.Value),
            };
            var allowedMutatedXenoGene = ModSettings.allowedMutatedXenoGene.Value;
            var minMetabolicEff = ModSettings.minimumMetabolicEffAllowed.Value;
            var allGenes = new List<GeneDef>(ModSettings.allGenes);
            var debug = ModSettings.debug.Value;

            if (debug)
            {
                var geneset = CreateGeneSetFromPawn(pawn);
                Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort}, current metabolic efficiency {geneset.MetabolismTotal}.");
                Log.Message($"MutatedPawn: {allGenes.Count} genes found.");
                Log.Message($"MutatedPawn: {ModSettings.disableViolenceGenes.Count} non-violent genes found: {string.Join(",", ModSettings.disableViolenceGenes)}");
            }
            RemoveUnwantedGenes(allGenes, pawn, request, debug);
            if (allGenes.Count < 1)
            {
                if (debug)
                {
                    Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} cannot have any mutations due no available gene left.");
                }
                return;
            }
            GeneSet chosenGenes = CreateGeneSetFromPawn(pawn);
            List<string> mutations = new List<string>();
            foreach ((var maxMutatedGenesAllowed, var percentChanceToHaveAMutatedGene) in chanceDictionary)
            {
                List<GeneDef> toBeRemovedFromAllGenes = new List<GeneDef>();
                var randomIndices = GenerateRandomIndices(allGenes.Count, maxMutatedGenesAllowed, debug);
                foreach (var index in randomIndices)
                {
                    if (!CanHaveMutatedGene(percentChanceToHaveAMutatedGene, debug))
                    {
                        continue;
                    }
                    GeneSet geneset = CreateGeneSetFromPawn(pawn);
                    var geneDef = allGenes[index];
                    geneset.AddGene(geneDef);
                    if (geneset.MetabolismTotal < minMetabolicEff)
                    {
                        if (debug)
                        {
                            Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} with gene {geneDef.defName} will result in a metabolic efficiency of {geneset.MetabolismTotal} (lower than {minMetabolicEff}). This gene is skipped.");
                        }
                        continue;
                    }
                    pawn.genes.AddGene(geneDef, IsXenoGene(allowedMutatedXenoGene, debug));
                    chosenGenes.AddGene(geneDef);
                    mutations.Add(geneDef.defName);
                    toBeRemovedFromAllGenes.Add(geneDef);
                    if (debug)
                    {
                        Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} have gene {geneDef.defName}, ({geneDef.biostatMet}) added, current metabolic efficiency {geneset.MetabolismTotal}.");
                    }
                    if (geneset.MetabolismTotal <= minMetabolicEff)
                    {
                        if (debug)
                        {
                            Log.Message($"MutatedPawn: Min metabolic effcient ({geneset.MetabolismTotal}) reached.");
                        }
                        break;
                    }
                }
                allGenes.RemoveAll(x => toBeRemovedFromAllGenes.Contains(x));
                if (allGenes.Count < 1 && debug)
                {
                    Log.Message($"MutatedPawn: No more available gene.");
                    break;
                }
            }
            if (mutations.Count < 1)
            {
                if (debug)
                {
                    Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} does not have any mutation.");
                }
                return;
            }
            HandleMutatedPawnComp(pawn, mutations, debug);
            if (debug)
            {
                Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} has a metabolic efficiency of {chosenGenes.MetabolismTotal} and mutated genes: {string.Join(",", mutations)}.");
            }
        }

        private static void HandleMutatedPawnComp(Pawn pawn, List<string> mutations, bool debug)
        {
            var mutatedPawnComp = pawn.GetComp<MutatedPawnComp>();
            if (mutatedPawnComp == null && debug)
            {
                Log.Message($"MutatedPawn: Pawn: {pawn.LabelShort} of def name {pawn.def.defName} has no mutated comp.");
                return;
            }
            mutatedPawnComp.MutationString = string.Join(",", mutations);
        }

        private static void RemoveUnwantedGenes(List<GeneDef> allGenes, Pawn pawn,
            PawnGenerationRequest request, bool debug)
        {
            var geneDefsForReading = pawn.genes.GenesListForReading.Select(x => x.def).ToList();
            allGenes.RemoveAll(x => geneDefsForReading.Contains(x));
            if (debug)
            {
                Log.Message($"MutatedPawn: Pawn: Remove pawn's genes from possible gene list. {allGenes.Count} genes left.");
            }
            if (request.MustBeCapableOfViolence)
            {
                allGenes.RemoveAll(x => ModSettings.disableViolenceGenes.Contains(x));
                if (debug)
                {
                    Log.Message($"MutatedPawn: Pawn must be capable of violent. {allGenes.Count} genes left.");
                }
            }
        }

        private static List<int> GenerateRandomIndices(int geneListLength, int maxMutatedGenesAllowed, bool debug)
        {
            var results = new List<int>();
            if (geneListLength <= maxMutatedGenesAllowed)
            {
                for (int i = 0; i < geneListLength; i++)
                {
                    results.Add(i);
                }
                return results;
            }
            while (results.Count < maxMutatedGenesAllowed)
            {
                float floatResult = UnityEngine.Random.Range(0, geneListLength);
                var intResult = (int)Math.Floor(floatResult);
                if (!results.Contains(intResult))
                {
                    results.Add(intResult);
                }
            }
            if (debug)
            {
                Log.Message($"MutatedPawn: Indexes rolled: {string.Join(",", results)}.");
            }
            return results;
        }

        private static bool CanHaveMutatedGene(int chanceToHaveAMutatedGene, bool debug)
        {
            float floatResult = UnityEngine.Random.Range(0f, 100f);
            var message = $"MutatedPawn: Mutation chance rolls {floatResult} (allowed chance {chanceToHaveAMutatedGene})";
            if (floatResult <= chanceToHaveAMutatedGene)
            {
                if (debug)
                {
                    Log.Message($"{message}. Allowed.");
                }
                return true;
            }
            if (debug)
            {
                Log.Message($"{message}. Skipped.");
            }
            return false;
        }

        private static bool IsXenoGene(bool allowedMutatedXenoGene, bool debug)
        {
            if (!allowedMutatedXenoGene)
            {
                if (debug)
                {
                    Log.Message($"MutatedPawn: Setting only allow engogenes.");
                }
                return false;
            }
            float chance = UnityEngine.Random.Range(0f, 1f);
            if (debug)
            {
                Log.Message($"MutatedPawn: Xenogene chance roll: {chance}.");
            }
            if (chance >= 0.5f)
            {
                return true;
            }
            return false;
        }

        private static GeneSet CreateGeneSetFromPawn(Pawn pawn)
        {
            var result = new GeneSet();
            foreach (var gene in pawn.genes.GenesListForReading)
            {
                result.AddGene(gene.def);
            }
            return result;
        }
    }
}
