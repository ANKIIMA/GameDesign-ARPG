namespace SharpUI.Source.Common.UI.Elements.State
{
    public interface IElementState
    {
        void Enable();
        void Disable();
        void Press();
        void Release();
        void Focus();
        void UnFocus();
        void SelectIfSelectable();
        void DeselectIfSelectable();
        void MakeSelectable();
        void MakeNonSelectable();
        void MakeClickable();
        void MakeNonClickable();
        
        bool IsSelectable();
        bool IsNonSelectable();
        bool IsSelected();
        bool IsDeselected();
        bool IsFocused();
        bool IsUnFocused();
        bool IsPressed();
        bool IsReleased();
        bool IsEnabled();
        bool IsDisabled();
        bool IsClickable();
        bool IsNotClickable();
    }
}