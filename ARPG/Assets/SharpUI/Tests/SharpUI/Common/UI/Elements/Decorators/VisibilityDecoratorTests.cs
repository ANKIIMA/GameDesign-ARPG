using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class VisibilityDecoratorTests
    {
        private VisibilityDecorator _decorator;
        private GameObject _gameObject;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _decorator = _gameObject.AddComponent<VisibilityDecorator>();
        }

        [Test]
        public void OnSelected_WhenVisibilityRequested_WillBeVisible()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnSelected();
            
            Assert.IsTrue(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnSelected_WhenVisibilityNotRequested_WillNotBeVisible()
        {
            _decorator.visibleWhenSelected = false;
            _gameObject.SetActive(false);
            
            _decorator.OnSelected();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnDeselected_WhenSelectedAndVisibilityRequested_WillBeVisible()
        {
            _decorator.visibleWhenSelected = true;
            _decorator.OnSelected();
            _gameObject.SetActive(true);
            
            _decorator.OnDeselected();

            Assert.IsFalse(_gameObject.activeSelf);
        }

        [Test]
        public void OnPressed_WillDoNothing()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnPressed();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnReleased_WillDoNothing()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnReleased();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnEnter_WillDoNothing()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnEnter();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnExit_WillDoNothing()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnExit();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnEnabled_WillDoNothing()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnEnabled();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
        
        [Test]
        public void OnDisabled_WillDoNothing()
        {
            _decorator.visibleWhenSelected = true;
            _gameObject.SetActive(false);
            
            _decorator.OnDisabled();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }
    }
}