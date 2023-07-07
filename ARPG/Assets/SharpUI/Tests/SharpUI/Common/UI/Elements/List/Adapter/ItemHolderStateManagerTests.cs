using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Adapter
{
    public class ItemHolderStateManagerTests
    {
        private const int ItemHolderCount = 25;
        private List<ItemHolder<string>> _itemHolders;
        private ISelectionStrategy<ItemHolder<string>> _selectionStrategy;
        private ItemHolderStateManager<string> _stateManager;

        [SetUp]
        public void SetUp()
        {
            _itemHolders = CreateItemHolders();
            _selectionStrategy = Substitute.For<ISelectionStrategy<ItemHolder<string>>>();
            _stateManager = new ItemHolderStateManager<string>(_itemHolders, _selectionStrategy);
        }

        private List<ItemHolder<string>> CreateItemHolders(
            bool enabled = true,
            bool clickable = true,
            bool selectable = true,
            bool selected = true)
        {
            var items = new List<ItemHolder<string>>();
            foreach (var _ in Enumerable.Range(1, ItemHolderCount))
            {
                var item = new GameObject().AddComponent<Item>();
                
                item.isClickable = clickable;
                item.isSelectable = selectable;
                item.isSelected = selected;
                item.Awake();
                if (enabled)
                    item.EnableButton();
                else
                    item.DisableButton();
                var itemHolder = new ItemHolder<string>(item);
                items.Add(itemHolder);
            }

            return items;
        }

        [Test]
        public void SetCanClickItems_ToTrue_WillSetHolderItemsClickable()
        {
            _itemHolders = CreateItemHolders(false);
            _stateManager = new ItemHolderStateManager<string>(_itemHolders, _selectionStrategy);
            
            _stateManager.SetCanClickItems(true);

            foreach (var holder in _itemHolders)
                Assert.IsTrue(holder.item.GetState().IsClickable());
        }
        
        [Test]
        public void SetCanClickItems_ToFalse_WillSetHolderItemsNonClickable()
        {
            _stateManager.SetCanClickItems(false);

            foreach (var holder in _itemHolders)
                Assert.IsTrue(holder.item.GetState().IsNotClickable());
        }

        [Test]
        public void SetCanSelectItems_AnyState_WillDeselectAll()
        {
            _stateManager.SetCanSelectItems(false);
            
            _stateManager.SetCanSelectItems(true);
            
            _selectionStrategy.Received(2).DeselectAll();
        }

        [Test]
        public void SetCanSelectItems_ToTrue_WillMakeItemsSelectable()
        {
            _itemHolders = CreateItemHolders(true, true, false);
            _stateManager = new ItemHolderStateManager<string>(_itemHolders, _selectionStrategy);
            
            _stateManager.SetCanSelectItems(true);
            
            foreach (var holder in _itemHolders)
                Assert.IsTrue(holder.item.GetState().IsSelectable());
        }
        
        [Test]
        public void SetCanSelectItems_ToFalse_WillMakeItemsNonSelectable()
        {
            _stateManager.SetCanSelectItems(false);
            
            foreach (var holder in _itemHolders)
                Assert.IsTrue(holder.item.GetState().IsNonSelectable());
        }

        [Test]
        public void SetItemsEnabled_ToTrue_WillEnableHolderItems()
        {
            _itemHolders = CreateItemHolders(false);
            _stateManager = new ItemHolderStateManager<string>(_itemHolders, _selectionStrategy);
            
            _stateManager.SetItemsEnabled(true);
            
            foreach (var holder in _itemHolders)
                Assert.IsTrue(holder.item.GetState().IsEnabled());
        }
        
        [Test]
        public void SetItemsEnabled_ToFalse_WillDisableHolderItems()
        {
            _stateManager.SetItemsEnabled(false);
            
            foreach (var holder in _itemHolders)
                Assert.IsTrue(holder.item.GetState().IsDisabled());
        }
        
        [Test]
        public void SetItemsEnabled_ToFalse_WillDisableSelected()
        {
            _stateManager.SetItemsEnabled(false);
            
            _selectionStrategy.Received().DeselectAll();
        }

        [Test]
        public void InitItemState_WhenAllStatesTrue_WillSetStateCorrectly()
        {
            var item = new GameObject().AddComponent<Item>();
            item.Awake();
            item.isClickable = false;
            item.isSelectable = false;
            item.EnableButton();
            _stateManager.SetCanClickItems(true);
            _stateManager.SetCanSelectItems(true);
            _stateManager.SetItemsEnabled(true);
            
            _stateManager.InitItemState(item);
            
            Assert.True(item.isClickable);
            Assert.True(item.isSelectable);
            Assert.True(item.GetState().IsEnabled());
            Assert.True(item.GetState().IsClickable());
            Assert.True(item.GetState().IsSelectable());
        }
        
        [Test]
        public void InitItemState_WhenAllStatesFalse_WillSetStateCorrectly()
        {
            var item = new GameObject().AddComponent<Item>();
            item.Awake();
            item.isClickable = true;
            item.isSelectable = true;
            item.DisableButton();
            _stateManager.SetCanClickItems(false);
            _stateManager.SetCanSelectItems(false);
            _stateManager.SetItemsEnabled(false);
            
            _stateManager.InitItemState(item);
            
            Assert.False(item.isClickable);
            Assert.False(item.isSelectable);
            Assert.True(item.GetState().IsDisabled());
            Assert.True(item.GetState().IsNotClickable());
            Assert.True(item.GetState().IsNonSelectable());
        }
    }
}