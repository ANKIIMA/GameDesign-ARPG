using System;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public interface ISelectionChangeListener<out TData> where TData: class
    {
        IObservable<TData> ObserveItemSelected();
        IObservable<TData> ObserveItemDeselected();
        IObservable<Unit> ObserveSelectionChange();
    }
}