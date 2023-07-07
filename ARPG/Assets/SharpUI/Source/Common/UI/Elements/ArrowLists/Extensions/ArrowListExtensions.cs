using SharpUI.Source.Common.UI.Elements.ArrowLists.Animation;

namespace SharpUI.Source.Common.UI.Elements.ArrowLists.Extensions
{
    public static class ArrowListExtensions
    {
        public static float DirectionMultiplier(this AnimateDirection direction)
            => direction == AnimateDirection.Left ? -1 : 1;
    }
}