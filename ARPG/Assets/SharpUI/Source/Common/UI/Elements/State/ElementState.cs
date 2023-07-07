namespace SharpUI.Source.Common.UI.Elements.State
{
    public class ElementState : IElementState
    {
        private bool _isSelectable;
        private bool _isClickable;
        
        private bool _isEnabled;
        private bool _isSelected;
        private bool _isFocused;
        private bool _isPressed;

        public ElementState()
        {
            _isSelectable = true;
            _isClickable = true;
            SetDefaults();
        }

        public ElementState(bool selectable = true, bool clickable = true, bool enabled = true, bool selected = false,
            bool focused = false, bool pressed = false)
        {
            _isSelectable = selectable;
            _isClickable = clickable;
            _isEnabled = enabled;
            _isSelected = selected;
            _isFocused = focused;
            _isPressed = pressed;
        }

        private void SetDefaults()
        {
            _isEnabled = true;
            _isSelected = false;
            _isPressed = false;
            _isFocused = false;
        }

        public void Enable() => SetDefaults();

        public void Disable() => _isEnabled = false;

        public void Press() => _isPressed = true;

        public void Release() => _isPressed = false;

        public void Focus() => _isFocused = true;
        
        public void UnFocus() => _isFocused = false;

        public void SelectIfSelectable()
        {
            if (_isSelectable)
                _isSelected = true;
        }
        
        public void DeselectIfSelectable()
        {
            if (_isSelectable)
                _isSelected = false;
        }

        public void MakeSelectable() => _isSelectable = true;
        
        public void MakeNonSelectable() => _isSelectable = false;

        public void MakeClickable() => _isClickable = true;

        public void MakeNonClickable() => _isClickable = false;

        public bool IsSelectable() => _isSelectable;

        public bool IsNonSelectable() => !_isSelectable;
        
        public bool IsSelected() => _isSelected;

        public bool IsDeselected() => !_isSelected;
        
        public bool IsFocused() => _isFocused;

        public bool IsUnFocused() => !_isFocused;
        
        public bool IsPressed() => _isPressed;

        public bool IsReleased() => !_isPressed;
        
        public bool IsEnabled() => _isEnabled;

        public bool IsDisabled() => !_isEnabled;
        
        public bool IsClickable() => _isClickable;

        public bool IsNotClickable() => !_isClickable;
    }
}