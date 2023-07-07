using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class OffsetDecoratorTests
    {
        private readonly Vector3 _localPosition = new Vector3(-5.7f, 6.0f, 9f);
        private readonly Vector3 _offset = new Vector3(3.4f, 5.6f, -9.5f);
        private GameObject _gameObject;
        private OffsetDecorator _decorator;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<RectTransform>().localPosition = _localPosition;
            _decorator = _gameObject.AddComponent<OffsetDecorator>();
            _decorator.isEnabled = true;
            _decorator.offset = _offset;
            
            _decorator.Awake();
        }
        
        [Test]
        public void OnPressed_NotEnabled_WillNotOffset()
        {
            _decorator.isEnabled = false;
            _decorator.decoratorMode = DecoratorMode.OnPressRelease;
            
            _decorator.OnPressed();

            Assert.AreEqual(_localPosition, _decorator.GetComponent<RectTransform>().localPosition);
        }

        [Test]
        public void OnPressed_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnPressRelease;
            
            _decorator.OnPressed();

            var expected = _localPosition + _offset;
            Assert.AreEqual(expected, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnReleased_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnPressRelease;
            
            _decorator.OnReleased();

            Assert.AreEqual(_localPosition, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnEnter_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnterExit;
            
            _decorator.OnEnter();

            var expected = _localPosition + _offset;
            Assert.AreEqual(expected, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnExit_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnterExit;
            
            _decorator.OnExit();

            Assert.AreEqual(_localPosition, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnSelected_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnSelectDeselect;
            
            _decorator.OnSelected();

            var expected = _localPosition + _offset;
            Assert.AreEqual(expected, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnDeselected_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnSelectDeselect;
            
            _decorator.OnDeselected();

            Assert.AreEqual(_localPosition, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnEnabled_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnableDisable;
            
            _decorator.OnEnabled();

            var expected = _localPosition + _offset;
            Assert.AreEqual(expected, _decorator.GetComponent<RectTransform>().localPosition);
        }
        
        [Test]
        public void OnDisabled_WillOffsetCorrectly()
        {
            _decorator.decoratorMode = DecoratorMode.OnEnableDisable;
            
            _decorator.OnDisabled();

            Assert.AreEqual(_localPosition, _decorator.GetComponent<RectTransform>().localPosition);
        }
    }
}