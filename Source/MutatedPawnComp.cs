using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnComp : ThingComp
    {
        public string MutationString = "";

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
    }
}
