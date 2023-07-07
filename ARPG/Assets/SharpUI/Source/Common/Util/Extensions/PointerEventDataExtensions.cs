using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Source.Common.Util.Extensions
{
    public static class PointerEventDataExtensions
    {
        public static bool CanDrag(this PointerEventData eventData, GameObject dragTrigger = null)
            => eventData.button == PointerEventData.InputButton.Left &&
               (dragTrigger == null || eventData.hovered.Contains(dragTrigger));
    }
}