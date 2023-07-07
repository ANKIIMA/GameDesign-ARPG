using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public abstract class ColorDecorator : BaseDecorator<Color>
    {
        [SerializeField] public Color normalColor = Color.white;
        [SerializeField] public Color disabledColor = Color.white;
        [SerializeField] public Color pressedColor = Color.white;
        [SerializeField] public Color selectedColor = Color.white;
        [SerializeField] public Color hoverColor = Color.white;

        protected override void DecoratePressed() => Decorate(pressedColor);

        protected override void DecorateReleased()
        {
            if (IsSelected()) Decorate(selectedColor);
            else if (IsInside()) Decorate(hoverColor);
            else Decorate(normalColor);
        }

        protected override void DecorateSelected() => Decorate(selectedColor);

        protected override void DecorateDeselected()
        {
            if (IsInside()) Decorate(hoverColor);
            else if (IsPressed()) Decorate(pressedColor);
            else Decorate(normalColor);
        }

        protected override void DecorateEnter() => Decorate(hoverColor);

        protected override void DecorateExit()
        {
            if (IsSelected()) Decorate(selectedColor);
            else Decorate(normalColor);
        }

        protected override void DecorateEnabled() => Decorate(normalColor);

        protected override void DecorateDisabled() => Decorate(disabledColor);
    }
}