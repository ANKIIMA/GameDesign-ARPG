using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Selection
{
    public class SelectionStrategyImplementationTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private const int MaxSelections = 3;
        private const string ItemName1 = "item_name_1";
        private const string ItemName2 = "item_name_2";
        private const string ItemName3 = "item_name_3";
        private const string ItemName4 = "item_name_4";
        private const string ItemName5 = "item_name_5";

        private SelectionStrategyNone<ItemHolder<string>, string> _strategyNone;
        private SelectionStrategySingle<ItemHolder<string>, string> _strategySingle;
        private SelectionStrategyLimited<ItemHolder<string>, string> _strategyLimited;
        private SelectionStrategyAll<ItemHolder<string>, string> _strategyAll;

        private Item _item;
        private ItemHolder<string> _holder1;
        private ItemHolder<string> _holder2;
        private ItemHolder<string> _holder3;
        private ItemHolder<string> _holder4;
        private ItemHolder<string> _holder5;
        private List<ItemHolder<string>> _holders;

        [SetUp]
        public void SetUp()
        {
            _item = CreateListItem();
            _strategyNone = new SelectionStrategyNone<ItemHolder<string>, string>();
            _strategySingle = new SelectionStrategySingle<ItemHolder<string>, string>();
            _strategyLimited = new SelectionStrategyLimited<ItemHolder<string>, string>(MaxSelections);
            _strategyAll = new SelectionStrategyAll<ItemHolder<string>, string>();
            SetUpHolders();
        }

        private static Item CreateListItem()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<Item>();
            return gameObject.GetComponent<Item>();
        }

        private void SetUpHolders()
        {
            _holder1 = new ItemHolder<string>(_item);
            _holder1.BindData(ItemName1);
            _holder2 = new ItemHolder<string>(_item);
            _holder2.BindData(ItemName2);
            _holder3 = new ItemHolder<string>(_item);
            _holder3.BindData(ItemName3);
            _holder4 = new ItemHolder<string>(_item);
            _holder4.BindData(ItemName4);
            _holder5 = new ItemHolder<string>(_item);
            _holder5.BindData(ItemName5);
            _holders = new List<ItemHolder<string>> {_holder1, _holder2, _holder3, _holder4, _holder5};
        }
        
        [Test]
        public void GetSelectionType_StrategyTypeNone_WillHaveCorrectType()
        {
            Assert.AreEqual(_strategyNone.GetSelectionType(), ItemSelectionType.None);
        }

        [Test]
        public void ItemClicked_StrategyTypeNone_WillSelectNone()
        {
            _strategyNone.ItemClicked(_holder1);

            Assert.IsFalse(_strategyNone.HasSelections());
        }

        [Test]
        public void GetSelectionType_StrategyTypeSingle_WillHaveCorrectType()
        {
            Assert.AreEqual(_strategySingle.GetSelectionType(), ItemSelectionType.Single);
        }

        [Test]
        public void ItemClicked_StrategyTypeSingle_WillSelectItem()
        {
            _strategySingle.ItemClicked(_holder1);
            
            Assert.AreEqual(_strategySingle.GetSelectedHolders().First(), _holder1);
        }
        
        [Test]
        public void ItemClicked_TwoTimes_StrategyTypeSingle_WillStaySelected()
        {
            _strategySingle.ItemClicked(_holder1);
            _strategySingle.ItemClicked(_holder1);
            
            Assert.AreEqual(_strategySingle.GetSelectedHolders().First(), _holder1);
        }

        [Test]
        public void ItemClicked_NewItem_StrategyTypeSingle_WillSelectNew()
        {
            _strategySingle.ItemClicked(_holder1);
            
            _strategySingle.ItemClicked(_holder2);
            
            Assert.AreEqual(_strategySingle.GetSelectedHolders().Count, 1);
            Assert.AreEqual(_strategySingle.GetSelectedHolders().First(), _holder2);
        }

        [Test]
        public void GetSelectionType_StrategyTypeLimited_WillHaveCorrectType()
        {
            Assert.AreEqual(_strategyLimited.GetSelectionType(), ItemSelectionType.Limited);
        }

        [Test]
        public void ItemClicked_WhenNotSelectedAndCanSelect_StrategyTypeLimited_WillSelect()
        {
            _strategyLimited.ItemClicked(_holder1);
            
            Assert.AreEqual(_strategyLimited.GetSelectedHolders().First(), _holder1);
        }

        [Test]
        public void ItemClicked_WhenClickOnSelected_StrategyTypeLimited_WillDeselectSelected()
        {
            _strategyLimited.ItemClicked(_holder1);
            _strategyLimited.ItemClicked(_holder1);
            
            Assert.AreEqual(_strategyLimited.GetSelectedHolders().Count, 0);
        }

        [Test]
        public void ItemClicked_WhenLimitReached_WillNotSelectAnyMore()
        {
            foreach (var holder in _holders)
                _strategyLimited.ItemClicked(holder);
            
            Assert.Greater(_holders.Count, MaxSelections);
            Assert.AreEqual(_strategyLimited.GetSelectedHolders().Count, MaxSelections);
        }

        [Test]
        public void GetSelectionType_StrategyTypeAll_WillHaveCorrectType()
        {
            Assert.AreEqual(_strategyAll.GetSelectionType(), ItemSelectionType.All);
        }
        
        [Test]
        public void GetMaxAmount_StrategyTypeAll_WillHaveMaxIntegerNumber()
        {
            Assert.AreEqual(_strategyAll.GetMaxAmount(), int.MaxValue);
        }

        [Test]
        public void ItemClicked_StrategyTypeAll_CanSelectAll()
        {
            foreach (var holder in _holders)
                _strategyAll.ItemClicked(holder);
            
            Assert.AreEqual(_strategyAll.GetSelectedHolders().Count, _holders.Count);
        }

        [Test]
        public void ItemClicked_StrategyTypeAll_CanDeselectAll()
        {
            foreach (var holder in _holders)
                _strategyAll.ItemClicked(holder);
            
            foreach (var holder in _holders)
                _strategyAll.ItemClicked(holder);
            
            Assert.AreEqual(_strategyAll.GetSelectedHolders().Count, 0);
        }

        [Test]
        public void GetSelectionChangeListener_ItemClicked_WillBeObserved()
        {
            string observedValue = null;
            _strategyAll.GetSelectionChangeListener<string>().ObserveItemSelected()
                .SubscribeWith(_disposable, value => observedValue = value);
            
            _strategyAll.ItemClicked(_holder1);
            
            Assert.AreEqual(observedValue, _holder1.GetData());
        }
        
        [Test]
        public void GetSelectionChangeListener_ItemDeselected_WillBeObserved()
        {
            string observedValue = null;
            _strategyAll.GetSelectionChangeListener<string>().ObserveItemDeselected()
                .SubscribeWith(_disposable, value => observedValue = value);
            
            _strategyAll.ItemClicked(_holder1);
            _strategyAll.ItemClicked(_holder1);

            Assert.AreEqual(observedValue, _holder1.GetData());
        }
        
        [Test]
        public void GetSelectionChangeListener_ItemClicked_WillObserveAllChanges()
        {
            var observedCount = 0;
            _strategyAll.GetSelectionChangeListener<string>().ObserveSelectionChange()
                .SubscribeWith(_disposable, _ => observedCount++);
            
            foreach (var holder in _holders)
                _strategyAll.ItemClicked(holder);
            
            foreach (var holder in _holders)
                _strategyAll.ItemClicked(holder);

            Assert.AreEqual(observedCount, _holders.Count*2);
        }
    }
}
