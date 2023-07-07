using SharpUI.Source.Common.UI.Util.Layout;
using SharpUI.Source.Common.Util.Reactive;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.TooltipInfo
{
    public interface ITooltip
    {
        void SetDelayObserver(IDelayObserver observer);
        void PositionPointerTo(PointerPosition pointerPosition);
        void OffsetPointerByPercentage(float offset);
        void Hide();
        void SetShowDelayTimeMillis(long millis);
        void ShowToLeftOf(RectTransform transform);
        void ShowToRightOf(RectTransform transform);
        void ShowAbove(RectTransform transform);
        void ShowBelow(RectTransform transform);
        void BindContent(RectTransform contentTransform);
        void SetMargins(Margin margin);
    }
}