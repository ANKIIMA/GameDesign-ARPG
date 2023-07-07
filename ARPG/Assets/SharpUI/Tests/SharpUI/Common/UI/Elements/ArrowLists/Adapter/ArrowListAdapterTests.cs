using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ArrowLists.Adapter
{
    public class ArrowListAdapterTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private static List<string> Data => new List<string> { "1", "2", "3", "4", "5", "6" };
        private ArrowListAdapter<string> _adapter;

        private class FakeArrowListAdapter : ArrowListAdapter<string>
        {
            public override string CurrentItem() => HasCurrentData() ? data[currentIndex] : null;
            public override string PreviousItem() => HasPreviousData() ? data[currentIndex - 1] : null;
            public override string NextItem() => HasNextData() ? data[currentIndex + 1] : null;
        }

        [SetUp]
        public void SetUp()
        {
            _adapter = new FakeArrowListAdapter();
        }

        [Test]
        public void SetData_WillSelectFirst()
        {
            _adapter.SetData(Data);
            
            Assert.AreSame(Data[0], _adapter.CurrentItem());
        }

        [Test]
        public void SetData_WillNotifyDataChangeObserver()
        {
            var observed = false;
            _adapter.ObserveDataChange().SubscribeWith(_disposable, _ => observed = true);
            
            _adapter.SetData(Data);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void ArrowListAdapter_EmptyData_CurrentItemIsCorrect()
        {
            Assert.IsNull(_adapter.CurrentItem());
            Assert.IsFalse(_adapter.HasCurrentData());
        }
        
        [Test]
        public void ArrowListAdapter_EmptyData_PreviousItemIsCorrect()
        {
            Assert.IsNull(_adapter.PreviousItem());
            Assert.IsFalse(_adapter.HasPreviousData());
        }
        
        [Test]
        public void ArrowListAdapter_EmptyData_NextItemIsCorrect()
        {
            Assert.IsNull(_adapter.NextItem());
            Assert.IsFalse(_adapter.HasNextData());
        }

        [Test]
        public void ArrowListAdapter_InitialData_CurrentItemIsFirst()
        {
            _adapter.SetData(Data);
            
            Assert.AreSame(_adapter.CurrentItem(), Data[0]);
        }
        
        [Test]
        public void ArrowListAdapter_InitialData_PreviousItemIsNull()
        {
            _adapter.SetData(Data);
            
            Assert.IsNull(_adapter.PreviousItem());
        }
        
        [Test]
        public void ArrowListAdapter_InitialData_NextItemIsSecond()
        {
            _adapter.SetData(Data);
            
            Assert.AreSame(_adapter.NextItem(), Data[1]);
        }
        
        [Test]
        public void ArrowListAdapter_InitialData_HasPreviousIsFalse()
        {
            _adapter.SetData(Data);
            
            Assert.IsFalse(_adapter.HasPreviousData());
        }
        
        [Test]
        public void ArrowListAdapter_InitialData_HasCurrentIsTrue()
        {
            _adapter.SetData(Data);
            
            Assert.IsTrue(_adapter.HasCurrentData());
        }
        
        [Test]
        public void ArrowListAdapter_InitialData_HasNextIsTrue()
        {
            _adapter.SetData(Data);
            
            Assert.IsTrue(_adapter.HasNextData());
        }

        [Test]
        public void DataCount_HasCorrectAmount()
        {
            _adapter.SetData(Data);
            
            Assert.AreEqual(Data.Count, _adapter.DataCount());
        }

        [Test]
        public void SelectPrevious_WhenPreviousNotAvailable_WillPreserveCurrentSelection()
        {
            _adapter.SetData(Data);
            
            _adapter.SelectPrevious();
            
            Assert.AreSame(Data[0], _adapter.CurrentItem());
        }

        [Test]
        public void SelectPrevious_WhenPreviousAvailable_WillSelectPrevious()
        {
            _adapter.SetData(Data);
            _adapter.SelectAt(2);
            
            _adapter.SelectPrevious();
            
            Assert.AreSame(Data[1], _adapter.CurrentItem());
        }

        [Test]
        public void SelectNext_WhenNextAvailable_WillSelectNext()
        {
            _adapter.SetData(Data);
            
            _adapter.SelectNext();
            
            Assert.AreSame(Data[1], _adapter.CurrentItem());
        }

        [Test]
        public void SelectNext_WhenNextNotAvailable_WillPreserveSelection()
        {
            _adapter.SetData(Data);
            _adapter.SelectAt(Data.Count-1);
            
            _adapter.SelectNext();
            
            Assert.AreSame(Data[Data.Count-1], _adapter.CurrentItem());
        }
        
        [Test]
        public void SelectPrevious_WhenPreviousAvailable_WillNotifySelectionChangeObserver()
        {
            _adapter.SetData(Data);
            _adapter.SelectAt(2);
            var observed = false;
            _adapter.ObserveSelectionChange().SubscribeWith(_disposable, _ => observed = true);
            
            _adapter.SelectPrevious();
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void SelectPrevious_WhenPreviousNotAvailable_WillNotNotifySelectionChangeObserver()
        {
            var observed = false;
            _adapter.ObserveSelectionChange().SubscribeWith(_disposable, _ => observed = true);
            _adapter.SetData(Data);
            
            _adapter.SelectPrevious();
            
            Assert.IsFalse(observed);
        }
        
        [Test]
        public void SelectNext_WhenNextAvailable_WillNotifySelectionChangeObserver()
        {
            var observed = false;
            _adapter.ObserveSelectionChange().SubscribeWith(_disposable, _ => observed = true);
            _adapter.SetData(Data);
            
            _adapter.SelectNext();
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void SelectNext_WhenNextNotAvailable_WillNotNotifySelectionChangeObserver()
        {
            _adapter.SetData(Data);
            _adapter.SelectAt(Data.Count-1);
            var observed = false;
            _adapter.ObserveSelectionChange().SubscribeWith(_disposable, _ => observed = true);
            
            _adapter.SelectNext();
            
            Assert.IsFalse(observed);
        }

        [Test]
        public void ClearAllAndNotify_WillClearDataAndIndex()
        {
            _adapter.SetData(Data);
            
            _adapter.ClearAllAndNotify();
            
            Assert.AreEqual(0, _adapter.DataCount());
            Assert.IsNull(_adapter.CurrentItem());
        }

        [Test]
        public void ClearAllAndNotify_WillNotifyDataChangeObserver()
        {
            _adapter.SetData(Data);
            var observed = false;
            _adapter.ObserveDataChange().SubscribeWith(_disposable, _ => observed = true);
            
            _adapter.ClearAllAndNotify();
            
            Assert.IsTrue(observed);
        }
    }
}