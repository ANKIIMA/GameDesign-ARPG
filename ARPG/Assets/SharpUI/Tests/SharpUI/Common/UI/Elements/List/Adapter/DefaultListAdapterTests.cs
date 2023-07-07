using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.UI.Factory;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Adapter
{
    public class DefaultListAdapterTests
    {
        private const int DataCount = 5;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private FakeDefaultAdapter _defaultFakeAdapter;
        private readonly List<string> _data = new List<string>(DataCount)
            { "data1", "data2", "data3", "data4", "data5" };

        public class FakeDefaultAdapter : DefaultListAdapter
        {
            public ItemHolder<string> FakeCreateListItemHolder()
            {
                return base.CreateListItemHolder();
            }

            public void FakeBindListItemHolder(ItemHolder<string> holder, int position)
            {
                base.BindListItemHolder(holder, position);
            }
        }
        
        [SetUp]
        public void SetUp()
        {
            _defaultFakeAdapter = CreateFakeDefaultItemAdapter();
            _defaultFakeAdapter.SetSelectionStrategy(ItemSelectionType.All);
        }

        private FakeDefaultAdapter CreateFakeDefaultItemAdapter()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<FakeDefaultAdapter>();
            var adapter = gameObject.GetComponent<FakeDefaultAdapter>();
            adapter.itemFactory = gameObject.AddComponent<ListItemFactory>();
            
            var lipGameObject = new GameObject();
            lipGameObject.AddComponent<ItemText>();
            adapter.itemFactory.listItemPrefab = lipGameObject;
            
            return adapter;
        }


        [Test]
        public void SetItemsAndNotify_WillNotifyDataChange()
        {
            var notified = false;
            _defaultFakeAdapter.ObserveDataChange().SubscribeWith(_disposables, _ => notified = true);
            
            _defaultFakeAdapter.SetItemsAndNotify(_data);
            
            Assert.IsTrue(notified);
        }

        [Test]
        public void SetItemsAndNotify_ReturnsCorrectDataCount()
        {
            _defaultFakeAdapter.SetItemsAndNotify(_data);
            
            Assert.AreEqual(DataCount, _defaultFakeAdapter.DataCount());
        }

        [Test]
        public void CreateListItemHolder_ReturnsItemHolder()
        {
            var holder = _defaultFakeAdapter.FakeCreateListItemHolder();
            
            Assert.IsInstanceOf<DefaultItemHolder>(holder);
        }

        [Test]
        public void BindListItemHolder_WillBindAll()
        {
            _defaultFakeAdapter.SetItemsAndNotify(_data);
            var itemText = new GameObject().AddComponent<ItemText>();
            itemText.textTitle = new GameObject().AddComponent<TextMeshProUGUI>();
            var holder = new DefaultItemHolder(itemText);

            for (var i = 0; i < _data.Count; i++)
            {
                _defaultFakeAdapter.FakeBindListItemHolder(holder, i);

                Assert.AreEqual(_data[i], itemText.textTitle.text);
            }
        }
    }
}