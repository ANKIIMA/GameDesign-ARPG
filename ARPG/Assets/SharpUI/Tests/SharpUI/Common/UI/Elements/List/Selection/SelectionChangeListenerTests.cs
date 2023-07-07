using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Selection
{
    public class SelectionChangeListenerTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private SelectionChangeListener<string> _selectionChangeListener;
        
        private const string Data = "string_data";
        private string _observedData;
        private bool _changed;

        [SetUp]
        public void SetUp()
        {
            _changed = false;
            _observedData = null;
            _selectionChangeListener = new SelectionChangeListener<string>();
        }

        [Test]
        public void OnItemSelected_WillBeObserved()
        {
            _selectionChangeListener.ObserveItemSelected().SubscribeWith(_disposable, 
                data => _observedData = data);
            
            _selectionChangeListener.OnItemSelected(Data);
            
            Assert.AreEqual(Data, _observedData);
        }

        [Test]
        public void OnItemDeselected_WillBeObserved()
        {
            _selectionChangeListener.ObserveItemDeselected().SubscribeWith(_disposable, 
                data => _observedData = data);
            
            _selectionChangeListener.OnItemDeselected(Data);
            
            Assert.AreEqual(Data, _observedData);
        }
        
        [Test]
        public void OnSelectionChanged_WillBeObserved()
        {
            _selectionChangeListener.ObserveSelectionChange().SubscribeWith(_disposable, 
                _ => _changed = true);
            
            _selectionChangeListener.OnSelectionChanged();
            
            Assert.IsTrue(_changed);
        }
    }
}
