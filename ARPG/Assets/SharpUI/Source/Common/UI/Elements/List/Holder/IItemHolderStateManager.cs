using System;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.List.Holder
{
    public interface IItemHolderStateManager<TData> where TData : class
    {
        void OnNewSelectionStrategy(ISelectionStrategy<ItemHolder<TData>> selectionStrategy);
        void SetCanClickItems(bool canClick);
        void SetCanSelectItems(bool canSelect);
        void SetItemsEnabled(bool itemsEnabled);
        void InitItemState(ListItem.Item item);
        IObservable<Unit> GetStateChangeObserver();
    }
}