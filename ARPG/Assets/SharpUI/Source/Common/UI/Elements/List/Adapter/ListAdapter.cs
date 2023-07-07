using System;
using System.Collections.Generic;
using System.Linq;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.List.Adapter
{
    public abstract class ListAdapter<TData> : MonoBehaviour, IListAdapter where TData : class
    {
        private readonly Subject<Unit> _dataSetChangedObserver;
        private readonly IItemHolderStateManager<TData> _itemHolderStateManager;
        private ISelectionStrategy<ItemHolder<TData>> _selectionStrategy;
        private readonly HashSet<ItemHolder<TData>> _holders = new HashSet<ItemHolder<TData>>();
        private List<TData> _data = new List<TData>();

        protected ListAdapter()
        {
            _dataSetChangedObserver = new Subject<Unit>();
            _selectionStrategy = SelectionStrategyFactory
                .CreateSelectionStrategy<ItemHolder<TData>, TData>(ItemSelectionType.None);
            _itemHolderStateManager = new ItemHolderStateManager<TData>(_holders, _selectionStrategy);
        }

        public int DataCount() => _data.Count;

        public int HoldersCount() => _holders.Count;

        protected void SetData(List<TData> newData)
        {
            ClearAll();
            _data = new List<TData>(newData);
        }

        protected TData GetData(int index) => _data[index];
        
        public bool HasSelectedItems() => _selectionStrategy.HasSelections();

        public void SetCanClickItems(bool canClick) => _itemHolderStateManager.SetCanClickItems(canClick);

        public void SetCanSelectItems(bool canSelect) => _itemHolderStateManager.SetCanSelectItems(canSelect);

        public void SetItemsEnabled(bool itemsEnabled) => _itemHolderStateManager.SetItemsEnabled(itemsEnabled);
        
        protected void NotifyDataSetChanged() => _dataSetChangedObserver.OnNext(Unit.Default);

        public IObservable<Unit> ObserveDataChange() => _dataSetChangedObserver;

        private void OnItemClicked(ItemHolder<TData> holder) => _selectionStrategy.ItemClicked(holder);
        
        public ISelectionChangeListener<TData> GetSelectionChangeListener()
            => _selectionStrategy.GetSelectionChangeListener<TData>();

        public IObservable<Unit> GetItemHolderStateObserver() => _itemHolderStateManager.GetStateChangeObserver();

        public List<TData> GetSelectedData()
            => (from holder in _selectionStrategy.GetSelectedHolders()
                select holder.GetData()).ToList();

        public void RemoveSelected()
        {
            foreach (var holder in _selectionStrategy.GetSelectedHolders())
            {
                DeselectAndDestroy(holder);
                DeleteItem(holder);
            }
        }

        private void DeselectAndDestroy(ItemHolder<TData> itemHolder)
        {
            _selectionStrategy.DeselectItem(itemHolder);
            DestroyImmediate(itemHolder.item.gameObject);
        }

        private void DeleteItem(ItemHolder<TData> itemHolder)
        {
            _data.Remove(itemHolder.GetData());
            _holders.Remove(itemHolder);
        }

        public void ClearAll()
        {
            foreach (var holder in _holders)
                DeselectAndDestroy(holder);
         
            _selectionStrategy.DeselectAll();
            _data.Clear();
            _holders.Clear();
        }

        public void RenderTo(int position, Transform containerTransform)
        {
            var holder = CreateListItemHolder();
            holder.item.transform.SetParent(containerTransform);
            _itemHolderStateManager.InitItemState(holder.item);
            holder.item.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ => OnItemClicked(holder));
            BindListItemHolder(holder, position);
            _holders.Add(holder);
        }

        public void SetSelectionStrategy(ItemSelectionType type, int amount = 0)
        {
            _selectionStrategy = SelectionStrategyFactory
                .CreateSelectionStrategy<ItemHolder<TData>, TData>(type, amount);
            _itemHolderStateManager.OnNewSelectionStrategy(_selectionStrategy);
        }

        protected abstract ItemHolder<TData> CreateListItemHolder();

        protected abstract void BindListItemHolder(ItemHolder<TData> holder, int position);
    }
}