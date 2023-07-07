using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Factory;
using SharpUI.Source.Common.UI.Prototype.SharpList;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Factory
{
    public class ListItemFactoryTests
    {
        private ListItemFactory _factory;
        private GameObject _listItemPrefab;
        private GameObject _listItemDescriptionPrefab;
        private GameObject _listItemImagePrefab;
        private GameObject _listItemCustom;

        [SetUp]
        public void SetUp()
        {
            _factory = new GameObject().AddComponent<ListItemFactory>();
            
            _listItemPrefab = new GameObject();
            _listItemPrefab.AddComponent<ItemText>();
            _factory.listItemPrefab = _listItemPrefab;

            _listItemDescriptionPrefab = new GameObject();
            _listItemDescriptionPrefab.AddComponent<ItemDescription>();
            _factory.listItemDescriptionPrefab = _listItemDescriptionPrefab;
            
            _listItemImagePrefab = new GameObject();
            _listItemImagePrefab.AddComponent<ItemImage>();
            _factory.listItemImagePrefab = _listItemImagePrefab;
            
            _listItemCustom = new GameObject();
            _listItemCustom.AddComponent<CustomItem>();
            _factory.listItemCustom = _listItemCustom;
        }

        [Test]
        public void CreateListItemText_WillCreateCorrectItem()
        {
            var itemText = _factory.CreateListItemText();
            
            Assert.IsInstanceOf<ItemText>(itemText);
        }
        
        [Test]
        public void CreateListItemDescription_WillCreateCorrectItem()
        {
            var itemDescription = _factory.CreateListItemDescription();
            
            Assert.IsInstanceOf<ItemDescription>(itemDescription);
        }
        
        [Test]
        public void CreateListItemImage_WillCreateCorrectItem()
        {
            var itemImage = _factory.CreateListItemImage();
            
            Assert.IsInstanceOf<ItemImage>(itemImage);
        }
        
        [Test]
        public void CreateCustomListItem_WillCreateCorrectItem()
        {
            var itemCustom = _factory.CreateCustomListItem();
            
            Assert.IsInstanceOf<CustomItem>(itemCustom);
        }
    }
}