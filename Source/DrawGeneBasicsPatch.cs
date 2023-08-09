//using HarmonyLib;
//using RimWorld;
//using System.Collections.Generic;
//using UnityEngine;
//using Verse;

//namespace Buggy.RimworldMod.MutatedPawn
//{
//    [HarmonyPatch]
//    [HarmonyPatch(typeof(GeneUIUtility), "DrawGeneBasics")]
//    public class DrawGeneBasicsPatch
//    {
//        public static void Postfix(GeneDef gene, Rect geneRect, GeneType geneType, bool doBackground, bool clickable, bool overridden)
//        {
//            List<(Vector2, Vector2)> sides = new List<(Vector2, Vector2)>()
//            {
//                (new Vector2(geneRect.xMin, geneRect.yMin), new Vector2(geneRect.xMin, geneRect.yMax)),
//                (new Vector2(geneRect.xMin, geneRect.yMax), new Vector2(geneRect.xMax, geneRect.yMax)),
//                (new Vector2(geneRect.xMax, geneRect.yMax), new Vector2(geneRect.xMax, geneRect.yMin)),
//                (new Vector2(geneRect.xMax, geneRect.yMin), new Vector2(geneRect.xMin, geneRect.yMin)),
//            };
//            foreach ((var start, var end) in sides)
//            {
//                Widgets.DrawLine(start, end, Color.yellow, 1f);
//            }
//        }
//    }
//}
