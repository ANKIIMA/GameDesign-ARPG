using UnityEngine.EventSystems;

namespace SharpUI.Source.Common.UI.Elements.Events
{
    public interface IElementEventPromoter
    {
        void ObservedPointerDown();

        void ObservedPointerUp();

        void ObservedClick();
        void ObservePointerClick(PointerEventData.InputButton inputButton);

        void ObservedPointerEnter();

        void ObservedPointerExit();

        void ObservedSelected();

        void ObservedDeselect();
    }
}