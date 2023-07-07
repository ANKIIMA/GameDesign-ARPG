using TMPro;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    [RequireComponent(typeof(TMP_Text))]
    public class ColorDecoratorForText : ColorDecorator
    {
        private TMP_Text _text;

        public void Awake()
        {
            _text = GetComponent<TMP_Text>();
            Decorate(normalColor);
        }

        protected override void Decorate(Color color)
        {
            _text.color = color;
        }
    }
}