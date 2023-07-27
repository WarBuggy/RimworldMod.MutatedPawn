using HarmonyLib;
using System.Reflection;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    [StaticConstructorOnStartup]
    public class MutatedPawnPatcher
    {
        static MutatedPawnPatcher()
        {
            Harmony val = new Harmony("Buggy.RimworldMod.MutatedPawn");
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            val.PatchAll(executingAssembly);
        }
    }
}
