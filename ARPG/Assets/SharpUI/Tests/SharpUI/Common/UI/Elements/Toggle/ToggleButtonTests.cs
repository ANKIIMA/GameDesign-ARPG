using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Toggle;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Toggle
{
    public class ToggleButtonTests
    {
        private ToggleButton _toggleButton;
        private UnityEngine.UI.Button _button;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            _toggleButton = gameObject.AddComponent<ToggleButton>();
            _button = _toggleButton.gameObject.GetComponent<UnityEngine.UI.Button>();
            _toggleButton.checkedImage = gameObject.AddComponent<Image>();
            _toggleButton.Awake();
            _toggleButton.Start();
        }

        [Test]
        public void ToggleOn_WillBeOn()
        {
            _toggleButton.isOn = false;
            
            _toggleButton.ToggleOn();
            
            Assert.IsTrue(_toggleButton.isOn);
        }

        [Test]
        public void ToggleOff_WillBeOff()
        {
            _toggleButton.isOn = true;
            
            _toggleButton.ToggleOff();
            
            Assert.IsFalse(_toggleButton.isOn);
        }

        [Test]
        public void EnableToggle_WillBeEnabled()
        {
            _button.interactable = false;
            _toggleButton.GetState().Disable();
            
            _toggleButton.EnableToggle();
            
            Assert.IsTrue(_toggleButton.GetState().IsEnabled());
        }

        [Test]
        public void DisableToggle_WillBeDisabled()
        {
            _button.interactable = true;
            _toggleButton.GetState().Enable();
            
            _toggleButton.DisableToggle();
            
            Assert.IsTrue(_toggleButton.GetState().IsDisabled());
        }
    }
}