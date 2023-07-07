using System;
using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.List.Holder
{
    public class ItemHolderStateManager<TData> : IItemHolderStateManager<TData> where TData : class
    {
        private readonly Subject<Unit> _stateChangeObserver = new Subject<Unit>();
        private readonly IEnumerable<ItemHolder<TData>> _holders;
        private ISelectionStrategy<ItemHolder<TData>> _selectionStrategy;
        private bool _itemsEnabled;
        private bool _canClickItems;
        private bool _canSelectItems;
        
        public ItemHolderStateManager(IEnumerable<ItemHolder<TData>> holders,
            ISelectionStrategy<ItemHolder<TData>> selectionStrategy)
        {
            _holders = holders;
            _selectionStrategy = selectionStrategy;
        }

        public void OnNewSelectionStrategy(ISelectionStrategy<ItemHolder<TData>> selectionStrategy)
            => _selectionStrategy = selectionStrategy;

        public IObservable<Unit> GetStateChangeObserver() => _stateChangeObserver;

        public void SetCanClickItems(bool canClick)
        {
            _canClickItems = canClick;
            foreach (var holder in _holders)
                holder.item.SetClickable(_canClickItems);
            _stateChangeObserver.OnNext(Unit.Default);
        }

        public void SetCanSelectItems(bool canSelect)
        {
            _canSelectItems = canSelect;
            _selectionStrategy.DeselectAll();
            foreach (var holder in _holders)
                holder.item.SetSelectable(_canSelectItems);
            _stateChangeObserver.OnNext(Unit.Default);
        }

        public void SetItemsEnabled(bool itemsEnabled)
        {
            _itemsEnabled = itemsEnabled;
            
            if (_itemsEnabled)
                EnableItems();
            else
                DisableItems();
            _stateChangeObserver.OnNext(Unit.Default);
        }

        private void EnableItems()
        {
            foreach (var holder in _holders)
                holder.item.EnableButton();
            _stateChangeObserver.OnNext(Unit.Default);
        }

        private void DisableItems()
        {
            _selectionStrategy.DeselectAll();
            foreach (var holder in _holders)
                holder.item.DisableButton();
            _stateChangeObserver.OnNext(Unit.Default);
        }
        
        public void InitItemState(ListItem.Item item)
        {
            if (_itemsEnabled)
                item.EnableButton();
            else
                item.DisableButton();
            item.SetClickable(_canClickItems);
            item.SetSelectable(_canClickItems && _canSelectItems);
            _stateChangeObserver.OnNext(Unit.Default);
        }
    }
}