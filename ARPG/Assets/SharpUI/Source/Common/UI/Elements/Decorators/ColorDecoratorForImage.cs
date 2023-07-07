using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    [RequireComponent(typeof(Image))]
    public class ColorDecoratorForImage : ColorDecorator
    {
        private Image _image;

        public void Awake()
        {
            _image = GetComponent<Image>();
            Decorate(normalColor);
        }

        protected override void Decorate(Color color)
        {
            _image.color = color;
        }
    }
}