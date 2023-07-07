using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    public class IconButton : RectButton
    {
        [SerializeField] public Image iconImage;

        public void SetIcon(Sprite sprite)
        {
            iconImage.sprite = sprite;
        }
    }
}