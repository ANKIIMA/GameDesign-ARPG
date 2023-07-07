using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Util.Layout
{
    public class LayoutHelper : ILayoutHelper
    {
        public void ForceRebuildLayoutImmediate(RectTransform rectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}