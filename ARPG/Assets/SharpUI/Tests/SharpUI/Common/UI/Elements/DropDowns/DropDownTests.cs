using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.DropDowns;
using SharpUI.Source.Common.UI.Elements.DropDowns.Adapters;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.DropDowns
{
    public class DropDownTests
    {
        private static readonly List<string> Data = new List<string> { "d1", "d2", "d3", "d4", "d5", "d6", "d7"};
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private IDropDownAdapter _adapter;
        private DropDown _dropDown;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TMP_Dropdown>();
            _dropDown = gameObject.AddComponent<DropDown>();
            _adapter = Substitute.For<IDropDownAdapter>();
            _adapter.GetOptionsData().Returns(new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("item1")
            });
            
            _dropDown.Awake();
            _dropDown.SetDisposable(_disposable);
            _dropDown.SetAdapter(_adapter);
        }

        [Test]
        public void SelectAtIndex_WillSelectCorrectElement()
        {
            var adapter = new DefaultDropDownAdapter();
            const int selectedIndex = 3;

            _dropDown.SetAdapter(adapter);
            adapter.SetData(Data);
            _dropDown.GetComponent<TMP_Dropdown>().onValueChanged.AsObservable().SubscribeWith(_disposable,
                index => Assert.AreEqual(selectedIndex, index));
            
            _dropDown.SelectAtIndex(selectedIndex);
        }

        [Test]
        public void OnDestroy_WillDispose()
        {
            _dropDown.OnDestroy();
            
            Assert.IsTrue(_disposable.IsDisposed);
        }
    }
}