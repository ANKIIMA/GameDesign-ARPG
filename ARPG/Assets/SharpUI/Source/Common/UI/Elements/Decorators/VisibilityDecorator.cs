using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public class VisibilityDecorator : BaseDecorator<ElementVisibility>
    {
        [SerializeField] public bool visibleWhenSelected = true;
        
        protected override void Decorate(ElementVisibility visibility)
        {
            gameObject.SetActive(visibility == ElementVisibility.Visible);
        }

        protected override void DecoratePressed() { /* Do nothing */ }

        protected override void DecorateReleased() { /* Do nothing */ }

        protected override void DecorateSelected() => DefaultDecoration();

        protected override void DecorateDeselected() => DefaultDecoration();

        protected override void DecorateEnter() { /* Do nothing */ }

        protected override void DecorateExit() { /* Do nothing */ }

        protected override void DecorateEnabled() { /* Do nothing */ }

        protected override void DecorateDisabled() { /* Do nothing */ }

        private void DefaultDecoration()
        {
            if (visibleWhenSelected && IsSelected())
                Decorate(ElementVisibility.Visible);
            else
                Decorate(ElementVisibility.Invisible);
        }
    }
}