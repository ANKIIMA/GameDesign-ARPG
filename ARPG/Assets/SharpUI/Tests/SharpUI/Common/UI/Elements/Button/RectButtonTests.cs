using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class RectButtonTests
    {
        private RectButton _rectButton;
        
        [SetUp]
        public void SetUp()
        {
            _rectButton = new GameObject().AddComponent<RectButton>();
            _rectButton.Awake();
        }
        
        [Test]
        public void SetupUI_IsEnabledTrue_WillBeInteractable()
        {
            _rectButton.isEnabled = true;
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

            _rectButton.Start();
            
            Assert.IsTrue(_rectButton.GetComponent<UnityEngine.UI.Button>().interactable);
        }
        
        [Test]
        public void SetupUI_IsEnabledFalse_WillBeNonInteractable()
        {
            _rectButton.isEnabled = false;
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

            _rectButton.Start();
            
            Assert.IsFalse(_rectButton.GetComponent<UnityEngine.UI.Button>().interactable);
        }

        [Test]
        public void EnableButton_WhenNotInteractable_WillBecomeInteractable()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

            _rectButton.EnableButton();
            
            Assert.IsTrue(_rectButton.GetComponent<UnityEngine.UI.Button>().interactable);
        }
        
        [Test]
        public void EnableButton_WhenInteractable_WillBecomeNonInteractable()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

            _rectButton.DisableButton();
            
            Assert.IsFalse(_rectButton.GetComponent<UnityEngine.UI.Button>().interactable);
        }

        [Test]
        public void EnableButton_SetsStateEnabled()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

            _rectButton.EnableButton();
            
            Assert.IsTrue(_rectButton.GetState().IsEnabled());
        }
        
        [Test]
        public void DisableButton_SetsStateDisabled()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

            _rectButton.DisableButton();
            
            Assert.IsTrue(_rectButton.GetState().IsDisabled());
        }

        [Test]
        public void EnableButton_InformsDispatcherToEnable()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

            _rectButton.EnableButton();
            
            _rectButton.GetEventDispatcher().OnEnabled();
        }
        
        [Test]
        public void DisableButton_InformsDispatcherToDisable()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

            _rectButton.DisableButton();
            
            _rectButton.GetEventDispatcher().OnDisabled();
        }

        [Test]
        public void EnableButton_InformsDecoratorsToDecorateEnabled()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

            _rectButton.EnableButton();
            
            foreach (var decorator in _rectButton.GetDecorators())
                Assert.IsTrue(decorator.IsEnabled());
        }
        
        [Test]
        public void DisableButton_InformsDecoratorsToDecorateDisabled()
        {
            _rectButton.GetComponent<UnityEngine.UI.Button>().interactable = true;

            _rectButton.DisableButton();
            
            foreach (var decorator in _rectButton.GetDecorators())
                Assert.IsTrue(decorator.IsDisabled());
        }
    }
}