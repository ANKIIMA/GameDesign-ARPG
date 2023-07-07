namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public interface IDecorator
    {
        void OnPressed();
        void OnReleased();
        void OnSelected();
        void OnDeselected();
        void OnEnter();
        void OnExit();
        void OnEnabled();
        void OnDisabled();

        bool IsPressed();
        bool IsReleased();
        bool IsSelected();
        bool IsDeselected();
        bool IsInside();
        bool IsOutside();
        bool IsEnabled();
        bool IsDisabled();
    }
}