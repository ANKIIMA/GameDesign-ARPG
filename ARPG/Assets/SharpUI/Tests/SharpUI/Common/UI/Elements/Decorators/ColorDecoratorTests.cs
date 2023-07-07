using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class ColorDecoratorTests
    {
        private static readonly Color UndefinedColor = Color.clear;
        private static readonly Color NormalColor = Color.white;
        private static readonly Color DisabledColor = Color.black;
        private static readonly Color PressedColor = Color.red;
        private static readonly Color SelectedColor = Color.green;
        private static readonly Color HoverColor = Color.blue;

        private class TestColorDecorator : ColorDecorator
        {
            public Color decoratedColor;

            public TestColorDecorator()
            {
                normalColor = NormalColor;
                disabledColor = DisabledColor;
                pressedColor = PressedColor;
                selectedColor = SelectedColor;
                hoverColor = HoverColor;
            }
            
            protected override void Decorate(Color color)
            {
                decoratedColor = color;
            }
        }
        
        private TestColorDecorator _colorDecorator;
        private ColorDecoratorForText _colorDecoratorForText;
        private ColorDecoratorForImage _colorDecoratorForImage;
        
        private static TestColorDecorator CreateTestColorDecorator(
            StateActive active = StateActive.Enabled,
            StateSelected selected = StateSelected.Deselected,
            StateHover hover = StateHover.Outside,
            StatePressed pressed = StatePressed.Released)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TestColorDecorator>();
            var testColorDecorator = gameObject.GetComponent<TestColorDecorator>();
            testColorDecorator.SetStates(active, selected, hover, pressed);
            return testColorDecorator;
        }

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TextMeshPro>();
            gameObject.AddComponent<ColorDecoratorForText>();
            _colorDecoratorForText = gameObject.GetComponent<ColorDecoratorForText>();
            
            gameObject = new GameObject();
            gameObject.AddComponent<ColorDecoratorForImage>();
            _colorDecoratorForImage = gameObject.GetComponent<ColorDecoratorForImage>();
        }

        [Test]
        public void OnPressed_WillDecoratePressedColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnPressed();

            Assert.AreEqual(_colorDecorator.decoratedColor, PressedColor);
        }

        [Test]
        public void OnReleased_WhenSelected_WillDecorateSelectedColor()
        {
            _colorDecorator = CreateTestColorDecorator(StateActive.Enabled, StateSelected.Selected);
            
            _colorDecorator.OnReleased();

            Assert.AreEqual(_colorDecorator.decoratedColor, SelectedColor);
        }

        [Test]
        public void OnReleased_WhenInside_WillDecorateHoverColor()
        {
            _colorDecorator = CreateTestColorDecorator(StateActive.Enabled, StateSelected.Deselected, StateHover.Inside);
            
            _colorDecorator.OnReleased();

            Assert.AreEqual(_colorDecorator.decoratedColor, HoverColor);
        }

        [Test]
        public void OnReleased_WhenDeselectedAndOutside_WillDecorateNormalColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnReleased();

            Assert.AreEqual(_colorDecorator.decoratedColor, NormalColor);
        }

        [Test]
        public void OnSelected_WillDecorateSelectedColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnSelected();

            Assert.AreEqual(_colorDecorator.decoratedColor, SelectedColor);
        }

        [Test]
        public void OnDeselected_WhenInside_WillDecorateHoverColor()
        {
            _colorDecorator = CreateTestColorDecorator(StateActive.Enabled, StateSelected.Deselected, StateHover.Inside);
            
            _colorDecorator.OnDeselected();

            Assert.AreEqual(_colorDecorator.decoratedColor, HoverColor);
        }

        [Test]
        public void OnDeselected_WhenPressed_WillDecoratePressedColor()
        {
            _colorDecorator = CreateTestColorDecorator(StateActive.Enabled, StateSelected.Deselected,
                StateHover.Outside, StatePressed.Pressed);
            
            _colorDecorator.OnDeselected();

            Assert.AreEqual(_colorDecorator.decoratedColor, PressedColor);
        }

        [Test]
        public void OnDeselected_WhenOutsideAndReleased_WillDecorateNormalColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnDeselected();

            Assert.AreEqual(_colorDecorator.decoratedColor, NormalColor);
        }

        [Test]
        public void OnEnter_WillDecorateHoverColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnEnter();

            Assert.AreEqual(_colorDecorator.decoratedColor, HoverColor);
        }

        [Test]
        public void OnExit_WhenSelected_WillDecorateSelectedColor()
        {
            _colorDecorator = CreateTestColorDecorator(StateActive.Enabled, StateSelected.Selected);
            
            _colorDecorator.OnExit();

            Assert.AreEqual(_colorDecorator.decoratedColor, SelectedColor);
        }
        
        [Test]
        public void OnExit_WhenDeselected_WillDecorateNormalColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnExit();

            Assert.AreEqual(_colorDecorator.decoratedColor, NormalColor);
        }

        [Test]
        public void OnEnabled_WillDecorateNormalColor()
        {
            _colorDecorator = CreateTestColorDecorator(StateActive.Disabled);
            
            _colorDecorator.OnEnabled();

            Assert.AreEqual(_colorDecorator.decoratedColor, NormalColor);
        }

        [Test]
        public void OnDisabled_WillDecorateDisabledColor()
        {
            _colorDecorator = CreateTestColorDecorator();
            
            _colorDecorator.OnDisabled();

            Assert.AreEqual(_colorDecorator.decoratedColor, DisabledColor);
        }

        [Test]
        public void ColorDecoratorForText_OnPressed_WillDecorateTextPressedColor()
        {
            _colorDecoratorForText.Awake();
            _colorDecoratorForText.pressedColor = Color.cyan;
            
            _colorDecoratorForText.OnPressed();
            
            Assert.AreEqual(
                _colorDecoratorForText.pressedColor,
                _colorDecoratorForText.GetComponent<TMP_Text>().color);
        }
        
        [Test]
        public void ColorDecoratorForText_Awake_WillDecorateTextNormalColor()
        {
            _colorDecoratorForText.GetComponent<TMP_Text>().color = UndefinedColor;
            
            _colorDecoratorForText.Awake();
            
            Assert.AreEqual(
                _colorDecoratorForText.normalColor,
                _colorDecoratorForText.GetComponent<TMP_Text>().color);
        }

        [Test]
        public void ColorDecoratorForImage_OnPressed_WillDecorateImageSpritePressedColor()
        {
            _colorDecoratorForImage.Awake();
            _colorDecoratorForImage.pressedColor = Color.green;
            
            _colorDecoratorForImage.OnPressed();
            
            Assert.AreEqual(
                _colorDecoratorForImage.pressedColor,
                _colorDecoratorForImage.GetComponent<Image>().color);
        }
        
        [Test]
        public void ColorDecoratorForImage_Awake_WillDecorateImageSpriteNormalColor()
        {
            _colorDecoratorForImage.GetComponent<Image>().color = UndefinedColor;
            
            _colorDecoratorForImage.Awake();
            
            Assert.AreEqual(
                _colorDecoratorForImage.normalColor,
                _colorDecoratorForImage.GetComponent<Image>().color);
        }
    }
}
