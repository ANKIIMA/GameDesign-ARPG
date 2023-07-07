using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class ButtonSelectionGroupTests
    {
        private const int SelectedIndex = 1;
        private RectButton _button;
        private ButtonSelectionGroup _group;
        private GameObject _groupGameObject;
        private List<RectButton> _buttons;

        [SetUp]
        public void SetUp()
        {
            _buttons = new List<RectButton>();
            _groupGameObject = new GameObject();
            _group = _groupGameObject.AddComponent<ButtonSelectionGroup>();
            
            AddButtonTo();
            AddButtonTo();
            AddButtonTo();

            _group.Awake();
        }

        private void AddButtonTo()
        {
            var go2 = new GameObject();
            _button = go2.AddComponent<RectButton>();
            _buttons.Add(_button);
            go2.transform.SetParent(_groupGameObject.transform);
            _button.isClickable = true;
            _button.isSelectable = true;
            _button.Awake();
            _button.Start();
        }

        [Test]
        public void Start_WhenSelectionRequested_WillSelect()
        {
            _group.selectButtonByIndex = true;
            _group.selectAtIndex = SelectedIndex;
            
            _group.Start();
            
            Assert.IsTrue(_buttons[SelectedIndex].GetState().IsSelected());
        }

        [Test]
        public void SelectAtIndex_WillSelectCorrectButton()
        {
            _group.Start();
            
            for (var index = _buttons.Count - 1; index >= 0; index--)
            {
                _group.SelectAtIndex(index);

                Assert.IsTrue(_buttons[index].GetState().IsSelected());
            }
        }

        [Test]
        public void SelectAtIndex_WhenOutOfRange_WillSelectNone()
        {
            _group.Start();
            
            _group.SelectAtIndex(int.MaxValue);
            
            _buttons.ForEach(button => Assert.IsFalse(button.GetState().IsSelected()));
        }
    }
}
