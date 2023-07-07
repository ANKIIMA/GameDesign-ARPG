using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.ListItem
{
    public class ItemTests
    {
        private Item _item;

        [SetUp]
        public void SetUp()
        {
            _item = new GameObject().AddComponent<Item>();
            _item.isClickable = true;
            _item.isSelectable = true;
            _item.isSelected = true;
            _item.Awake();
        }

        [Test]
        public void Start_WhenSelected_WillObserveSelected()
        {
            _item.Start();
            
            Assert.IsTrue(_item.GetState().IsSelected());
        }
    }
}