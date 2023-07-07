using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using TMPro;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Holder
{
    public class DefaultListItemHolderTests
    {
        private DefaultItemHolder _holder;
        private ItemText _itemText;
        private const string Data = "string_data";

        [SetUp]
        public void SetUp()
        {
            _itemText = new GameObject().AddComponent<ItemText>();
            _itemText.textTitle = new GameObject().AddComponent<TextMeshPro>();
            _holder = new DefaultItemHolder(_itemText);
        }

        [Test]
        public void BindData_WillSaveData()
        {
            _holder.BindData(Data);
            
            Assert.AreEqual(_holder.GetData(), Data);
        }
    }
}
