using UnityEngine;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnMod : Mod
    {
        private readonly Settings _settings;

        public MutatedPawnMod(ModContentPack content)
            : base(content)
        {
            _settings = base.GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.Label("Buggy_MP_Option_LoreFriendly".Translate());
            listingStandard.Gap();

            listingStandard.Label($"{"Buggy_MP_Option_MaxMutatedGenesAllowed".Translate()}: {_settings.maxMutatedGenesAllowed}",
                tooltip: "Buggy_MP_Option_MaxMutatedGenesAllowed_Tooltip".Translate());
            _settings.maxMutatedGenesAllowed = 
                (int) listingStandard.Slider(_settings.maxMutatedGenesAllowed, 0f, 10f);

            listingStandard.Label($"{"Buggy_MP_Option_ChanceToHaveAMutatedGene".Translate()}: {_settings.percentChanceToHaveAMutatedGene}%",
                tooltip: "Buggy_MP_Option_ChanceToHaveAMutatedGene_Tooltip".Translate());
            _settings.percentChanceToHaveAMutatedGene = 
                (int) listingStandard.Slider(_settings.percentChanceToHaveAMutatedGene, 0f, 100f);
            listingStandard.GapLine();
                        
            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Label("Buggy_MP_Option_NoneLoreFriendly".Translate());
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Buggy_MP_Option_AllowedMutatedXenoGene".Translate(), ref _settings.allowedMutatedXenoGene,
                "Buggy_MP_Option_AllowedMutatedXenoGene_Tooltip".Translate());
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Buggy_MP_Option_AllowedMutatedArchiteGenes".Translate(), ref _settings.allowedMutatedArchiteGenes,
                "Buggy_MP_Option_AllowedMutatedArchiteGenes_Tooltip".Translate());
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Buggy_MP_Option_AllowedLowerThanMinMetabolicEff".Translate(), ref _settings.allowedLowerThanMinMetabolicEff,
                "Buggy_MP_Option_AllowedLowerThanMinMetabolicEff_Tooltip".Translate());
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Buggy_MP_Option_Debug".Translate(), ref _settings.debug);

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
    }

        public override string SettingsCategory()
        {
            return "Buggy_MP_Mod_Name".Translate();
        }
    }
}