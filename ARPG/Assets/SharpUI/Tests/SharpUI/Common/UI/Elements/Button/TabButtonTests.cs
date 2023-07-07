using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class TabButtonTests
    {
        private TabButton _tabButton;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            _tabButton = gameObject.AddComponent<TabButton>();
            _tabButton.content = new GameObject();
            
            _tabButton.Awake();
            _tabButton.Start();
        }

        [Test]
        public void WhenSelected_WillShowContent()
        {
            _tabButton.content.SetActive(false);
            
            _tabButton.GetEventDispatcher().OnSelect();
            
            Assert.IsTrue(_tabButton.content.activeSelf);
        }
        
        [Test]
        public void WhenDeselected_WillHideContent()
        {
            _tabButton.content.SetActive(true);
            
            _tabButton.GetEventDispatcher().OnDeselect();
            
            Assert.IsFalse(_tabButton.content.activeSelf);
        }
    }
}