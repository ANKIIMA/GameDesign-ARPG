using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.DropDowns.Adapters;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.DropDowns.Adapters
{
    public class DropDownAdapterTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private static readonly List<string> Data = new List<string> { "d1", "d2", "d3", "d4", "d5", "d6", "d7"};
        private FakeAdapter _adapter;

        private class FakeAdapter : DropDownAdapter<string>
        {
            public override string GetItemTextAt(int index) => Data[index];
        }

        [SetUp]
        public void SetUp()
        {
            _adapter = new FakeAdapter();
        }

        [Test]
        public void DataCount_IsCorrect()
        {
            _adapter.SetData(Data);
            
            Assert.AreEqual(Data.Count, _adapter.DataCount());
        }
        
        [Test]
        public void ObserveDataChange_WillNotifyDataChange()
        {
            var observed = false;
            _adapter.ObserveDataChange().SubscribeWith(_disposable, _ => observed = true);
            
            _adapter.SetData(Data);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void GetOptionsData_WillCreateCorrectData()
        {
            _adapter.SetData(Data);

            var data = _adapter.GetOptionsData();
            for (var index = 0; index < Data.Count; index++)
                Assert.AreEqual(Data[index], data[index].text);
        }

        [Test]
        public void GetItemTextAt_WillReturnCorrectData()
        {
            for (var index = 0; index < Data.Count; index++)
                Assert.AreEqual(Data[index], _adapter.GetItemTextAt(index));
        }
    }
}