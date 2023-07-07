using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public enum StateActive { Enabled, Disabled }
    public enum StateSelected { Selected, Deselected }
    public enum StateHover { Inside, Outside }
    public enum StatePressed { Pressed, Released }
            
    public abstract class BaseDecorator<TComponent> : MonoBehaviour, IDecorator
    {
        private StateActive _stateActive = StateActive.Enabled;
        private StateSelected _stateSelected = StateSelected.Deselected;
        private StateHover _stateHover = StateHover.Outside;
        private StatePressed _statePressed = StatePressed.Released;

        protected BaseDecorator() => SetDefaults();

        public void SetStates(StateActive active, StateSelected selected, StateHover hover, StatePressed pressed)
        {
            _stateActive = active;
            _stateSelected = selected;
            _stateHover = hover;
            _statePressed = pressed;
        }

        private void SetDefaults()
        {
            _stateActive = StateActive.Enabled;
            _stateSelected = StateSelected.Deselected;
            _stateHover = StateHover.Outside;
            _statePressed = StatePressed.Released;
        }

        public virtual void OnPressed()
        {
            if (IsDisabled()) return;
            
            _statePressed = StatePressed.Pressed;
            DecoratePressed();
        }

        public virtual void OnReleased()
        {
            if (IsDisabled()) return;
            
            _statePressed = StatePressed.Released;
            DecorateReleased();
        }

        public virtual void OnSelected()
        {
            if (IsDisabled()) return;
            
            _stateSelected = StateSelected.Selected;
            DecorateSelected();
        }

        public virtual void OnDeselected()
        {
            if (IsDisabled()) return;
            
            _stateSelected = StateSelected.Deselected;
            DecorateDeselected();
        }

        public virtual void OnEnter()
        {
            if (IsDisabled() || IsSelected()) return;
            
            _stateHover = StateHover.Inside;
            DecorateEnter();
        }

        public virtual void OnExit()
        {
            if (IsDisabled()) return;
            
            _stateHover = StateHover.Outside;
            DecorateExit();
        }

        public virtual void OnEnabled()
        {
            SetDefaults();
            DecorateEnabled();
        }

        public virtual void OnDisabled()
        {
            SetDefaults();
            _stateActive = StateActive.Disabled;
            DecorateDisabled();
        }

        public bool IsPressed() => _statePressed == StatePressed.Pressed;

        public bool IsReleased() => _statePressed == StatePressed.Released;

        public bool IsSelected() => _stateSelected == StateSelected.Selected;

        public bool IsDeselected() => _stateSelected == StateSelected.Deselected;

        public bool IsInside() => _stateHover == StateHover.Inside;

        public bool IsOutside() => _stateHover == StateHover.Outside;

        public bool IsEnabled() => _stateActive == StateActive.Enabled;

        public bool IsDisabled() => _stateActive == StateActive.Disabled;

        protected abstract void Decorate(TComponent component);
        
        protected abstract void DecoratePressed();
        protected abstract void DecorateReleased();
        protected abstract void DecorateSelected();
        protected abstract void DecorateDeselected();
        protected abstract void DecorateEnter();
        protected abstract void DecorateExit();
        protected abstract void DecorateEnabled();
        protected abstract void DecorateDisabled();
    }
}