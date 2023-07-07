using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class SpriteDecoratorTests
    {
        private static readonly Rect DefaultRect = new Rect(1, 1, 1, 1);
        private static readonly Vector2 DefaultVector = Vector2.zero;
        private readonly Sprite _undefinedSprite = Sprite.Create(Texture2D.normalTexture, Rect.zero, Vector2.negativeInfinity);
        private readonly Sprite _normalSprite = Sprite.Create(Texture2D.blackTexture, DefaultRect, DefaultVector);
        private readonly Sprite _disabledSprite = Sprite.Create(Texture2D.grayTexture, DefaultRect, DefaultVector);
        private readonly Sprite _pressedSprite = Sprite.Create(Texture2D.redTexture, DefaultRect, DefaultVector);
        private readonly Sprite _selectedSprite = Sprite.Create(Texture2D.whiteTexture, DefaultRect, DefaultVector);
        private readonly Sprite _hoverSprite = Sprite.Create(Texture2D.linearGrayTexture, DefaultRect, DefaultVector);

        private SpriteDecorator _spriteDecorator;
        
        private SpriteDecorator CreateTestSpriteDecorator(
            StateActive active = StateActive.Enabled,
            StateSelected selected = StateSelected.Deselected,
            StateHover hover = StateHover.Outside,
            StatePressed pressed = StatePressed.Released)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<SpriteDecorator>();
            var testSpriteDecorator = gameObject.GetComponent<SpriteDecorator>();
            testSpriteDecorator.SetStates(active, selected, hover, pressed);
            testSpriteDecorator.normalSprite = _normalSprite;
            testSpriteDecorator.disabledSprite = _disabledSprite;
            testSpriteDecorator.pressedSprite = _pressedSprite;
            testSpriteDecorator.selectedSprite = _selectedSprite;
            testSpriteDecorator.hoverSprite = _hoverSprite;
            testSpriteDecorator.Awake();
            return testSpriteDecorator;
        }

        [Test]
        public void OnPressed_WillDecoratePressedSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();

            _spriteDecorator.OnPressed();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _pressedSprite);
        }

        [Test]
        public void OnReleased_WhenSelected_WillDecorateSelectedSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator(StateActive.Enabled, StateSelected.Selected);
            
            _spriteDecorator.OnReleased();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _selectedSprite);
        }

        [Test]
        public void OnReleased_WhenInside_WillDecorateHoverSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator(StateActive.Enabled, StateSelected.Deselected,
                StateHover.Inside);
            
            _spriteDecorator.OnReleased();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _hoverSprite);
        }

        [Test]
        public void OnReleased_WhenDeselectedAndOutside_WillDecorateNormalSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            
            _spriteDecorator.OnReleased();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _normalSprite);
        }
        
        [Test]
        public void OnSelected_WillDecorateSelectedSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            
            _spriteDecorator.OnSelected();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _selectedSprite);
        }

        [Test]
        public void OnDeselected_WhenInside_WillDecorateHoverSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator(StateActive.Enabled, StateSelected.Deselected,
                StateHover.Inside);
            
            _spriteDecorator.OnDeselected();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _hoverSprite);
        }

        [Test]
        public void OnDeselected_WhenPressed_WillDecoratePressedSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator(StateActive.Enabled, StateSelected.Deselected,
                StateHover.Outside,
                StatePressed.Pressed);
            
            _spriteDecorator.OnDeselected();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _pressedSprite);
        }

        [Test]
        public void OnDeselected_WhenOutsideAndReleased_WillDecorateNormalSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            
            _spriteDecorator.OnDeselected();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _normalSprite);
        }

        [Test]
        public void OnEnter_WillDecorateHoverSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            
            _spriteDecorator.OnEnter();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _hoverSprite);
        }

        [Test]
        public void OnExit_WhenSelected_WillDecorateSelectedSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator(StateActive.Enabled, StateSelected.Selected);
            
            _spriteDecorator.OnExit();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _selectedSprite);
        }
        
        [Test]
        public void OnExit_WhenDeselected_WillDecorateNormalSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            
            _spriteDecorator.OnExit();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _normalSprite);
        }

        [Test]
        public void OnEnabled_WillDecorateNormalSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator(StateActive.Disabled);
            
            _spriteDecorator.OnEnabled();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _normalSprite);
        }

        [Test]
        public void OnDisabled_WillDecorateDisabledSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            
            _spriteDecorator.OnDisabled();

            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _disabledSprite);
        }

        [Test]
        public void Awake_WillDecorateImageNormalSprite()
        {
            _spriteDecorator = CreateTestSpriteDecorator();
            _spriteDecorator.GetComponent<Image>().sprite = _undefinedSprite;
            
            _spriteDecorator.Awake();
            
            Assert.AreEqual(_spriteDecorator.GetComponent<Image>().sprite, _normalSprite);
        }
    }
}
