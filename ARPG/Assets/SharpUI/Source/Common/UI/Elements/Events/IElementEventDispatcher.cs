namespace SharpUI.Source.Common.UI.Elements.Events
{
    public interface IElementEventDispatcher
    {
        void OnPressed();
        void OnReleased();
        void OnClicked();
        void OnLeftClicked();
        void OnRightClicked();
        void OnMiddleClicked();
        void OnEnabled();
        void OnDisabled();
        void OnEnter();
        void OnExit();
        void OnSelect();
        void OnDeselect();
    }
}