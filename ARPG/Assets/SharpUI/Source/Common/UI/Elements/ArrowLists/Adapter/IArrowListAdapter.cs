using System;
using JetBrains.Annotations;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter
{
    public interface IArrowListAdapter
    {
        [CanBeNull] string CurrentItem();
        [CanBeNull] string PreviousItem();
        [CanBeNull] string NextItem();
        
        int DataCount();
        void ClearAllAndNotify();
        IObservable<Unit> ObserveDataChange();
        IObservable<Unit> ObserveSelectionChange();
        bool HasCurrentData();
        bool HasPreviousData();
        bool HasNextData();
        bool HasDataAtIndex(int index);
        void SelectAt(int index);
        void SelectPrevious();
        void SelectNext();
    }
}