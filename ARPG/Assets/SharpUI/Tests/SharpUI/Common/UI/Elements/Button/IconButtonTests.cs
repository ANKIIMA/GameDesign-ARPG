using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class IconButtonTests
    {
        private static readonly Rect DefaultRect = new Rect(1, 1, 1, 1);
        private static readonly Vector2 DefaultVector = Vector2.zero;
        private readonly Sprite _sprite = Sprite.Create(Texture2D.blackTexture, DefaultRect, DefaultVector);
        
        private IconButton _iconButton;

        [SetUp]
        public void SetUp()
        {
            _iconButton = new GameObject().AddComponent<IconButton>();
            _iconButton.iconImage = new GameObject().AddComponent<Image>();
        }

        [Test]
        public void SetIcon_WillSetSpriteOnImage()
        {
            _iconButton.SetIcon(_sprite);
            
            Assert.AreEqual(_iconButton.iconImage.sprite, _sprite);
        }
    }
}