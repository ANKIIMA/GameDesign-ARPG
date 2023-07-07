using SharpUI.Source.Common.UI.Elements.Decorators;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    public class RectButton : BaseButton
    {
        protected override void SetupUI()
        {
            base.SetupUI();
            SetEnabled(isEnabled);
        }
        
        private void SetEnabled(bool stateEnabled)
        {
            isEnabled = stateEnabled;
            
            if (stateEnabled)
                EnableButton();
            else
                DisableButton();
        }

        public void EnableButton()
        {
            if (button.interactable) return;
            
            button.interactable = true;
            state.Enable();
            dispatcher.OnEnabled();
            decorators.OnEnabled();
        }

        public void DisableButton()
        {
            if (button.interactable == false) return;
            
            button.interactable = false;
            state.Disable();
            dispatcher.OnDisabled();
            decorators.OnDisabled();
        }
    }
}