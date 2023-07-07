using System;
using System.Collections.Generic;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter
{
    public abstract class ArrowListAdapter<TData> : IArrowListAdapter where TData : class
    {
        private readonly Subject<Unit> _dataChangeObserver;
        private readonly Subject<Unit> _selectionChangeObserver;
        protected List<TData> data = new List<TData>();
        protected int currentIndex;

        protected ArrowListAdapter()
        {
            currentIndex = -1;
            _dataChangeObserver = new Subject<Unit>();
            _selectionChangeObserver = new Subject<Unit>();
        }
        
        public void SetData(IEnumerable<TData> newData)
        {
            data = new List<TData>(newData);
            TrySelectFirst();
            NotifyDataSetChanged();
        }

        private void TrySelectFirst()
        {
            if (data.Count > 0)
                currentIndex = 0;
        }

        public int DataCount() => data.Count;
        
        private void NotifyDataSetChanged() => _dataChangeObserver.OnNext(Unit.Default);

        public IObservable<Unit> ObserveDataChange() => _dataChangeObserver;

        public IObservable<Unit> ObserveSelectionChange() => _selectionChangeObserver;
        
        public bool HasCurrentData() => currentIndex != -1;

        public bool HasPreviousData() => currentIndex > 0;

        public bool HasNextData() => data.Count > 2 && currentIndex < data.Count - 1;

        public bool HasDataAtIndex(int index) => index < data.Count;

        public void SelectAt(int index)
        {
            if (!HasDataAtIndex(index)) return;

            currentIndex = index;
            _selectionChangeObserver.OnNext(Unit.Default);
        }

        public void SelectPrevious()
        {
            if (!HasPreviousData()) return;
            
            currentIndex--;
            _selectionChangeObserver.OnNext(Unit.Default);
        }

        public void SelectNext()
        {
            if (!HasNextData()) return;
            
            currentIndex++;
            _selectionChangeObserver.OnNext(Unit.Default);
        }

        public void ClearAllAndNotify()
        {
            data.Clear();
            currentIndex = -1;
            NotifyDataSetChanged();
        }

        public abstract string CurrentItem();

        public abstract string PreviousItem();

        public abstract string NextItem();
    }
}