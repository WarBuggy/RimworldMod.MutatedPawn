using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(GeneUIUtility))]
    public class GeneUIUtilityPatch
    {
        private static List<string> Mutations = new List<string>();

        [HarmonyPatch("DrawGeneSections")]
        [HarmonyPrefix]
        public static void DrawGeneSectionsPrefix(Rect rect, Thing target, GeneSet genesOverride, ref Vector2 scrollPosition)
        {
            Mutations = new List<string>();
            if (target is Pawn pawn)
            {
                var mutatedPawnComp = pawn.GetComp<MutatedPawnComp>();
                if (mutatedPawnComp == null)
                {
                    return;
                }
                Mutations = mutatedPawnComp.CreateMutationList();
            }
        }

        [HarmonyPatch("DrawGeneBasics")]
        [HarmonyPostfix]
        public static void DrawGeneBasicsPostfix(GeneDef gene, Rect geneRect, GeneType geneType, bool doBackground, bool clickable, bool overridden)
        {
            if (Mutations.Count < 1)
            {
                return;
            }
            if (!Mutations.Contains(gene.defName))  
            {
                return;
            }
            List<(Vector2, Vector2)> sides = new List<(Vector2, Vector2)>()
            {
                (new Vector2(geneRect.xMin, geneRect.yMin), new Vector2(geneRect.xMin, geneRect.yMax)),
                (new Vector2(geneRect.xMin, geneRect.yMax), new Vector2(geneRect.xMax, geneRect.yMax)),
                (new Vector2(geneRect.xMax, geneRect.yMax), new Vector2(geneRect.xMax, geneRect.yMin)),
                (new Vector2(geneRect.xMax, geneRect.yMin), new Vector2(geneRect.xMin, geneRect.yMin)),
            };
            foreach ((var start, var end) in sides)
            {
                Widgets.DrawLine(start, end, Color.yellow, 1f);
            }
        }
    }
}
