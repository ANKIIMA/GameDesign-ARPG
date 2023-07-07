using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Adapter
{
    public class ListAdapterTests
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private const int DataCount = 5;
        private List<string> _data;

        public class FakeListAdapter : ListAdapter<string>
        {
            public List<ItemHolder<string>> fakeHolders = new List<ItemHolder<string>>();

            public void SetDataAndNotify(List<string> newData)
            {
                SetData(newData);
                NotifyDataSetChanged();
            }

            protected override ItemHolder<string> CreateListItemHolder()
            {
                var item = new GameObject().AddComponent<ItemText>();
                item.Awake();
                var holder = new ItemHolder<string>(item);
                fakeHolders.Add(holder);
                return holder;
            }

            protected override void BindListItemHolder(ItemHolder<string> holder, int position)
            {
                holder.BindData(GetData(position));
            }
        }

        private FakeListAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _data = new List<string> { "data1", "data2", "data3", "data4", "data5" };
            _adapter = new GameObject().AddComponent<FakeListAdapter>().GetComponent<FakeListAdapter>();
            _adapter.SetDataAndNotify(_data);
            _adapter.SetSelectionStrategy(ItemSelectionType.All);
        }
        
        private void RenderItems()
        {
            for (var i = 0; i < _adapter.DataCount(); i++)
                _adapter.RenderTo(i, _adapter.gameObject.transform);
        }

        [Test]
        public void RenderTo_WillCreateItemHolders()
        {
            RenderItems();
            
            Assert.AreEqual(DataCount, _adapter.HoldersCount());
        }
        
        [Test]
        public void RenderTo_WillBindData()
        {
            RenderItems();

            for (var i = 0; i < _adapter.fakeHolders.Count; i++)
                Assert.AreEqual(_data[i], _adapter.fakeHolders[i].GetData());
        }

        [Test]
        public void ItemHolder_OnClicked_WhenItemsSelectable_WillSelectAll()
        {
            RenderItems();

            foreach (var holder in _adapter.fakeHolders)
                holder.item.GetEventDispatcher().OnClicked();
            
            Assert.AreEqual(DataCount, _adapter.GetSelectedData().Count);
        }
        
        [Test]
        public void ItemHolder_OnClicked_WhenItemsSelectable_WillListen()
        {
            RenderItems();
            var observed = false;
            _adapter.GetSelectionChangeListener().ObserveSelectionChange().SubscribeWith(_disposables,
                _ => observed = true);

            foreach (var holder in _adapter.fakeHolders)
                holder.item.GetEventDispatcher().OnClicked();
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void RemoveSelected_WillRemoveSelectedData()
        {
            RenderItems();
            const int selectCount = DataCount - 2;
            for (var i = 0; i < selectCount; i++)
                _adapter.fakeHolders[i].item.GetEventDispatcher().OnClicked();

            _adapter.RemoveSelected();
            
            Assert.AreEqual(DataCount - selectCount, _adapter.DataCount());
        }
        
        [Test]
        public void RemoveSelected_WillRemoveSelectedHolders()
        {
            RenderItems();
            const int selectCount = DataCount - 2;
            for (var i = 0; i < selectCount; i++)
                _adapter.fakeHolders[i].item.GetEventDispatcher().OnClicked();

            _adapter.RemoveSelected();
            
            Assert.AreEqual(DataCount - selectCount, _adapter.HoldersCount());
        }

        [Test]
        public void ClearAll_WillRemoveAllData()
        {
            RenderItems();
            foreach (var holder in _adapter.fakeHolders)
                holder.item.GetEventDispatcher().OnClicked();
            
            _adapter.ClearAll();
            
            Assert.AreEqual(0, _adapter.DataCount());
        }
        
        [Test]
        public void ClearAll_WillRemoveAllItemHolders()
        {
            RenderItems();
            foreach (var holder in _adapter.fakeHolders)
                holder.item.GetEventDispatcher().OnClicked();
            
            _adapter.ClearAll();
            
            Assert.AreEqual(0, _adapter.HoldersCount());
        }
        
        [Test]
        public void ClearAll_WillDeselectAllData()
        {
            RenderItems();
            foreach (var holder in _adapter.fakeHolders)
                holder.item.GetEventDispatcher().OnClicked();
            
            _adapter.ClearAll();
            
            Assert.IsFalse(_adapter.HasSelectedItems());
        }
        
        [Test]
        public void SetCanClickItems_ToTrue_WillBeObserved()
        {
            var observed = false;
            _adapter.GetItemHolderStateObserver().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetCanClickItems(true);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void SetCanClickItems_ToFalse_WillBeObserved()
        {
            var observed = false;
            _adapter.GetItemHolderStateObserver().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetCanClickItems(false);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void SetCanSelectItems_ToTrue_WillBeObserved()
        {
            var observed = false;
            _adapter.GetItemHolderStateObserver().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetCanSelectItems(true);
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void SetCanSelectItems_ToFalse_WillBeObserved()
        {
            var observed = false;
            _adapter.GetItemHolderStateObserver().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetCanSelectItems(false);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void SetItemsEnabled_ToTrue_WillBeObserved()
        {
            var observed = false;
            _adapter.GetItemHolderStateObserver().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetItemsEnabled(true);
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void SetItemsEnabled_ToFalse_WillBeObserved()
        {
            var observed = false;
            _adapter.GetItemHolderStateObserver().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetItemsEnabled(false);
            
            Assert.IsTrue(observed);
        }
    }
}