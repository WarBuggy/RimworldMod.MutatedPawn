using UnityEngine;
using Verse;

namespace Buggy.RimworldMod.MutatedPawn
{
    public class Listing_Settings : Listing_Standard
    {
        private const float ScrollAreaWidth = 24f;

        public void BeginScrollView(Rect rect, ref Vector2 scrollPosition, ref Rect viewRect)
        {
            if (viewRect == default) { viewRect = new Rect(rect.x, rect.y, rect.width - ScrollAreaWidth, 99999f); }

            Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);

            Begin(viewRect);
        }

        public void EndScrollView(ref Rect viewRect)
        {
            End();
            Widgets.EndScrollView();
            viewRect.height = CurHeight;
        }
    }
}