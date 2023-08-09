using UnityEngine;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class MutatedPawnMod : Mod
    {
        private readonly Settings _settings;

        private static Vector2 _scrollPosition = Vector2.zero;
        private static Rect _viewRect;


        public MutatedPawnMod(ModContentPack content)
            : base(content)
        {
            _settings = base.GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            inRect.yMax -= 60f;
            DrawSettings(inRect.ContractedBy(8f, 0f));
            base.DoSettingsWindowContents(inRect);
        }

        public void DrawSettings(Rect rect)
        {
            var listingRect = new Rect(rect.x, rect.y + 5f, rect.width, rect.height - 5f);
            var listingStandard = new Listing_Settings();
            listingStandard.Begin(rect);
            listingStandard.End();
            listingStandard.BeginScrollView(listingRect, ref _scrollPosition, ref _viewRect);

            listingStandard.Gap();
            listingStandard.Label("Buggy_MP_Option_LoreFriendly".Translate());
            listingStandard.Gap();

            listingStandard.Label("Buggy_MP_Option_1stMutationChance".Translate());
            listingStandard.Label($"{"Buggy_MP_Option_MaxNumberOfMutation".Translate()}: {_settings.maxMutatedGenesAllowed1stChance}",
                tooltip: "Buggy_MP_Buggy_MP_Option_MaxNumberOfMutation_1stGroup_Tooltip".Translate());
            _settings.maxMutatedGenesAllowed1stChance =
                (int)listingStandard.Slider(_settings.maxMutatedGenesAllowed1stChance, 0f, 10f);
            listingStandard.Label($"{"Buggy_MP_Option_ChanceToHaveAMutatedGene".Translate()}: {_settings.percentChanceToHaveAMutatedGene1stChance}%",
                tooltip: "Buggy_MP_Option_ChanceToHaveAMutatedGene_1stGroup_Tooltip".Translate());
            _settings.percentChanceToHaveAMutatedGene1stChance =
                (int)listingStandard.Slider(_settings.percentChanceToHaveAMutatedGene1stChance, 0f, 100f);

            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.Label("Buggy_MP_Option_2ndMutationChance".Translate());
            listingStandard.Label($"{"Buggy_MP_Option_MaxNumberOfMutation".Translate()}: {_settings.maxMutatedGenesAllowed2ndChance}",
                tooltip: "Buggy_MP_Buggy_MP_Option_MaxNumberOfMutation_2ndGroup_Tooltip".Translate());
            _settings.maxMutatedGenesAllowed2ndChance =
                (int)listingStandard.Slider(_settings.maxMutatedGenesAllowed2ndChance, 0f, 10f);
            listingStandard.Label($"{"Buggy_MP_Option_ChanceToHaveAMutatedGene".Translate()}: {_settings.percentChanceToHaveAMutatedGene2ndChance}%",
                tooltip: "Buggy_MP_Option_ChanceToHaveAMutatedGene_2ndGroup_Tooltip".Translate());
            _settings.percentChanceToHaveAMutatedGene2ndChance =
                (int)listingStandard.Slider(_settings.percentChanceToHaveAMutatedGene2ndChance, 0f, 100f);

            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.Label("Buggy_MP_Option_3rdMutationChance".Translate());
            listingStandard.Label($"{"Buggy_MP_Option_MaxNumberOfMutation".Translate()}: {_settings.maxMutatedGenesAllowed3rdChance}",
                tooltip: "Buggy_MP_Buggy_MP_Option_MaxNumberOfMutation_3rdGroup_Tooltip".Translate());
            _settings.maxMutatedGenesAllowed3rdChance =
                (int)listingStandard.Slider(_settings.maxMutatedGenesAllowed3rdChance, 0f, 10f);
            listingStandard.Label($"{"Buggy_MP_Option_ChanceToHaveAMutatedGene".Translate()}: {_settings.percentChanceToHaveAMutatedGene3rdChance}%",
                tooltip: "Buggy_MP_Option_ChanceToHaveAMutatedGene_3rdGroup_Tooltip".Translate());
            _settings.percentChanceToHaveAMutatedGene3rdChance =
                (int)listingStandard.Slider(_settings.percentChanceToHaveAMutatedGene3rdChance, 0f, 100f);

            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.Label($"{"Buggy_MP_Option_ChanceWithCarcinomaGrowing".Translate()}: {_settings.chanceWhenCarcinomaGrowing}%",
                tooltip: "Buggy_MP_Option_ChanceWithCarcinomaGrowing_Tooltip".Translate());
            _settings.chanceWhenCarcinomaGrowing =
               (int)listingStandard.Slider(_settings.chanceWhenCarcinomaGrowing, 0f, 100f);

            listingStandard.Gap();
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

            listingStandard.Label($"{"Buggy_MP_Option_MinimumMetabolicEffForMutatedGen".Translate()}: {_settings.minimumMetabolicEffAllowed}",
                tooltip: "Buggy_MP_Option_MinimumMetabolicEffForMutatedGen_Tooltip".Translate());
            _settings.minimumMetabolicEffAllowed =
               (int)listingStandard.Slider(_settings.minimumMetabolicEffAllowed, -30f, 5f);
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("Buggy_MP_Option_Debug".Translate(), ref _settings.debug);

            listingStandard.EndScrollView(ref _viewRect);
        }

        public override string SettingsCategory()
        {
            return "Buggy_MP_Mod_Name".Translate();
        }
    }
}