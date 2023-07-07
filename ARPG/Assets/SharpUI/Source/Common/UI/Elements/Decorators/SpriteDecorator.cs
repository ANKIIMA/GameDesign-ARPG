using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    [RequireComponent(typeof(Image))]
    public sealed class SpriteDecorator : BaseDecorator<Sprite>
    {
        [SerializeField] public Sprite normalSprite;
        [SerializeField] public Sprite disabledSprite;
        [SerializeField] public Sprite pressedSprite;
        [SerializeField] public Sprite selectedSprite;
        [SerializeField] public Sprite hoverSprite;
        
        private Image _image;

        public void Awake()
        {
            _image = GetComponent<Image>();
            Decorate(normalSprite);
        }
        
        protected override void Decorate(Sprite sprite)
        {
            if (!Equals(_image.sprite, sprite))
                _image.sprite = sprite;
        }

        protected override void DecoratePressed() => Decorate(pressedSprite);

        protected override void DecorateReleased()
        {
            if (IsSelected()) Decorate(selectedSprite);
            else if (IsInside()) Decorate(hoverSprite);
            else Decorate(normalSprite);
        }

        protected override void DecorateSelected() => Decorate(selectedSprite);

        protected override void DecorateDeselected()
        {
            if (IsInside()) Decorate(hoverSprite);
            else if (IsPressed()) Decorate(pressedSprite);
            else Decorate(normalSprite);
        }

        protected override void DecorateEnter() => Decorate(hoverSprite);

        protected override void DecorateExit()
        {
            if (IsSelected()) Decorate(selectedSprite);
            else Decorate(normalSprite);
        }

        protected override void DecorateEnabled() => Decorate(normalSprite);

        protected override void DecorateDisabled() => Decorate(disabledSprite);
    }
}