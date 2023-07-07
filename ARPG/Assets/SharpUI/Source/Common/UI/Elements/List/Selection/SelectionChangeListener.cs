using System;
using JetBrains.Annotations;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public class SelectionChangeListener<TData> : ISelectionChangeListener<TData>
        where TData : class
    {
        [CanBeNull] private Subject<TData> _itemSelectedObserver;
        [CanBeNull] private Subject<TData> _itemDeselectedObserver;
        [CanBeNull] private Subject<Unit> _selectionChangeObserver;

        public void OnItemSelected(TData item) => _itemSelectedObserver?.OnNext(item);

        public void OnItemDeselected(TData item) => _itemDeselectedObserver?.OnNext(item);

        public void OnSelectionChanged() => _selectionChangeObserver?.OnNext(Unit.Default);

        public IObservable<TData> ObserveItemSelected()
            => _itemSelectedObserver ?? (_itemSelectedObserver = new Subject<TData>());

        public IObservable<TData> ObserveItemDeselected()
            => _itemDeselectedObserver ?? (_itemDeselectedObserver = new Subject<TData>());

        public IObservable<Unit> ObserveSelectionChange()
            => _selectionChangeObserver ?? (_selectionChangeObserver = new Subject<Unit>());
    }
}