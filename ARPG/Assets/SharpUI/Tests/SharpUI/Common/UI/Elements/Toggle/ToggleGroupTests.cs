using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Toggle;
using UnityEngine;
using UnityEngine.UI;
using ToggleGroup = SharpUI.Source.Common.UI.Elements.Toggle.ToggleGroup;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Toggle
{
    public class ToggleGroupTests
    {
        private ToggleGroup _toggleGroup;

        private ToggleButton _toggleButton1;
        private ToggleButton _toggleButton2;

        private const int ToggleButtonCount = 4;

        [SetUp]
        public void SetUp()
        {
            CreateToggleGroup();
            _toggleGroup.Awake();
        }

        private void CreateToggleGroup()
        {
            var gameObject = new GameObject();
            _toggleGroup = gameObject.AddComponent<ToggleGroup>();
            _toggleButton1 = gameObject.AddComponent<ToggleButton>(); // # 1
            _toggleButton1.checkedImage = new GameObject().AddComponent<Image>();
            _toggleButton2 = gameObject.AddComponent<ToggleButton>(); // # 2
            _toggleButton2.checkedImage = new GameObject().AddComponent<Image>();
            gameObject.AddComponent<ToggleButton>(); // # 3
            gameObject.AddComponent<ToggleButton>(); // # 4
        }

        [Test]
        public void Awake_WillLoadButtons()
        {
            Assert.AreEqual(ToggleButtonCount, _toggleGroup.ButtonCount());
        }

        [Test]
        public void Awake_WillHaveNoSelectedIndex()
        {
            Assert.AreEqual(ushort.MaxValue, _toggleGroup.GetSelectedIndex());
        }

        [Test]
        public void Awake_WillHaveNoSelectedButton()
        {
            Assert.IsNull(_toggleGroup.GetSelectedButton());
        }

        [Test]
        public void Start_WillSelectByIndexWhenRequested()
        {
            _toggleGroup.selectByIndex = true;
            _toggleGroup.selectedIndex = 1;
            
            _toggleGroup.Start();
            
            Assert.AreSame(_toggleGroup.GetSelectedButton(), _toggleButton2);
        }

        [Test]
        public void ToggleWorks()
        {
            var button = _toggleButton1.GetComponent<UnityEngine.UI.Button>();
            _toggleButton1.isClickable = true;
            _toggleButton1.isSelectable = true;
            _toggleButton1.Awake();
            _toggleButton1.Start();
            
            Assert.IsFalse(_toggleButton1.isOn); // Not selected
            button.onClick.Invoke();
            Assert.IsTrue(_toggleButton1.isOn); // Selected
            button.onClick.Invoke();
            Assert.IsTrue(_toggleButton1.isOn); // Stays selected
        }
    }
}