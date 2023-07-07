using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using UnityEngine;
using Random = System.Random;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List
{
    public class ListViewTests
    {
        private IListAdapter _adapter;
        private ListView _listView;
        private GameObject _container;
        private GameObject _listViewGameObject;

        [SetUp]
        public void SetUp()
        {
            _adapter = Substitute.For<IListAdapter>();
            _adapter.DataCount().Returns(1);
            
            _listViewGameObject = new GameObject();
            _listViewGameObject.AddComponent<ListView>();

            _container = new GameObject();
            _listView = _listViewGameObject.GetComponent<ListView>();
            _listView.container = _container;
            _listView.selectionType = ItemSelectionType.Single;
            _listView.SetAdapter(_adapter);
            
            _listView.Awake();
        }
        
        private static bool RandomBoolean() => new Random().Next(0, 2) > 0;

        [Test]
        public void SetItemsEnabled_WillNotifyAdapter()
        {
            var value = RandomBoolean();
            
            _listView.SetItemsEnabled(value);
            
            _adapter.Received().SetItemsEnabled(value);
        }

        [Test]
        public void SetItemsClickable_WillNotifyAdapter()
        {
            var value = RandomBoolean();
            
            _listView.SetItemsClickable(value);
            
            _adapter.Received().SetCanClickItems(value);
        }

        [Test]
        public void SetItemsSelectable_WillNotifyAdapter()
        {
            var value = RandomBoolean();
            
            _listView.SetItemsSelectable(value);
            
            _adapter.Received().SetCanSelectItems(value);
        }

        [Test]
        public void Awake_WillInitAdapter()
        {
            _listViewGameObject.AddComponent<DefaultListAdapter>();
            _listView.Awake();
            
            Assert.NotNull(_listView.GetComponent<IListAdapter>());
        }
    }
}
