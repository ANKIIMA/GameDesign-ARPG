using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Layout
{
    public interface ILayoutHelper
    {
        void ForceRebuildLayoutImmediate(RectTransform rectTransform);
    }
}