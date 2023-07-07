using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class ScaleDecoratorTests
    {
        private const float ActiveScale = 0.678f;
        private static readonly Vector3 ActiveScaleVector = new Vector3(ActiveScale, ActiveScale, ActiveScale);
        private const float DefaultScale = 0.899f;
        private static readonly Vector3 DefaultScaleVector = new Vector3(DefaultScale, DefaultScale, DefaultScale);
        
        private GameObject _gameObject;
        private ScaleDecorator _decorator;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<RectTransform>();
            _decorator = _gameObject.AddComponent<ScaleDecorator>();
            _decorator.isEnabled = true;
            _decorator.scaleType = ScaleDecorator.ScaleType.ScaleXYZ;
            _decorator.activeScale = ActiveScale;
            _decorator.defaultScale = DefaultScale;
            
            _decorator.Awake();
        }
        
        [Test]
        public void OnPressed_WhenModePressReleaseAndDisabled_WillNotScaleToActive()
        {
            _decorator.isEnabled = false;
            _decorator.decoratorMode = DecoratorMode.OnPressRelease;

            _decorator.OnPressed();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreNotEqual(ActiveScaleVector, scale);
        }

        [Test]
        public void OnPressed_WhenModePressRelease_WillScaleToActive()
        {
            _decorator.decoratorMode = DecoratorMode.OnPressRelease;

            _decorator.OnPressed();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(ActiveScaleVector, scale);
        }
        
        [Test]
        public void OnPressed_WhenModeNotPressRelease_WillDoNothing()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnterExit;

            _decorator.OnPressed();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(Vector3.one, scale);
        }

        [Test]
        public void OnReleased_WhenModePressRelease_WillDecorateDefault()
        {
            _decorator.decoratorMode = DecoratorMode.OnPressRelease;

            _decorator.OnReleased();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(DefaultScaleVector, scale);
        }
        
        [Test]
        public void OnSelected_WhenModeSelectDeselect_WillScaleToActive()
        {
            _decorator.decoratorMode = DecoratorMode.OnSelectDeselect;

            _decorator.OnSelected();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(ActiveScaleVector, scale);
        }
        
        [Test]
        public void OnSelected_WhenModeNotSelectDeselect_WillDoNothing()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnterExit;

            _decorator.OnSelected();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(Vector3.one, scale);
        }

        [Test]
        public void OnDeselected_WhenModeNotSelectDeselect_WillScaleToDefault()
        {
            _decorator.decoratorMode = DecoratorMode.OnSelectDeselect;

            _decorator.OnDeselected();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(DefaultScaleVector, scale);
        }
        
        [Test]
        public void OnEnter_WhenModeEnterExit_WillScaleToActive()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnterExit;

            _decorator.OnEnter();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(ActiveScaleVector, scale);
        }
        
        [Test]
        public void OnEnter_WhenModeNotEnterExit_WillDoNothing()
        {
            _decorator.decoratorMode = DecoratorMode.OnSelectDeselect;

            _decorator.OnEnter();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(Vector3.one, scale);
        }

        [Test]
        public void OnExit_WhenModeEnterExit_WillScaleToDefault()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnterExit;

            _decorator.OnExit();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(DefaultScaleVector, scale);
        }
        
        [Test]
        public void OnEnable_WhenModeEnableDisable_WillScaleToActive()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnableDisable;

            _decorator.OnEnabled();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(ActiveScaleVector, scale);
        }
        
        [Test]
        public void OnEnabled_WhenModeNotEnableDisable_WillDoNothing()
        {
            _decorator.decoratorMode = DecoratorMode.OnSelectDeselect;

            _decorator.OnEnabled();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(Vector3.one, scale);
        }

        [Test]
        public void OnDisabled_WhenModeEnableDisable_WillScaleToDefault()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnableDisable;

            _decorator.OnDisabled();

            var scale = _decorator.GetComponent<RectTransform>().localScale;
            Assert.AreEqual(DefaultScaleVector, scale);
        }
    }
}