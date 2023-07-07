using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public abstract class SelectionStrategy<THolder, TData> : ISelectionStrategy<THolder>
        where THolder : ItemHolder<TData>
        where TData : class
    {
        private readonly ItemSelectionType _type;
        private readonly int _maxAmount;
        private readonly HashSet<THolder> _selectedHolders = new HashSet<THolder>();
        private readonly SelectionChangeListener<TData> _selectionChangeListener;

        protected SelectionStrategy(ItemSelectionType type = ItemSelectionType.None, int amount = 0)
        {
            _type = type;
            _maxAmount = amount;
            _selectionChangeListener = new SelectionChangeListener<TData>();
        }

        protected bool CanSelect() => _selectedHolders.Count < _maxAmount;

        public ItemSelectionType GetSelectionType() => _type;

        public int GetMaxAmount() => _maxAmount;

        public abstract void ItemClicked(THolder holder);
        public bool HasSelections() => _selectedHolders.Count > 0;

        public ISelectionChangeListener<T> GetSelectionChangeListener<T>() where T : class
            => (ISelectionChangeListener<T>) _selectionChangeListener;

        protected void SelectItem(THolder holder)
        {
            _selectedHolders.Add(holder);
            holder.item.SelectItem();
            _selectionChangeListener.OnItemSelected(holder.GetData());
            _selectionChangeListener.OnSelectionChanged();
        }

        public void DeselectItem(THolder holder)
        {
            _selectedHolders.Remove(holder);
            holder.item.DeselectItem();
            _selectionChangeListener.OnItemDeselected(holder.GetData());
            _selectionChangeListener.OnSelectionChanged();
        }

        public void DeselectAll()
        {
            foreach (var holder in _selectedHolders)
            {
                holder.item.DeselectItem();
                _selectionChangeListener.OnItemDeselected(holder.GetData());
            }

            _selectedHolders.Clear();
            _selectionChangeListener.OnSelectionChanged();
        }

        protected bool IsSelected(THolder holder) => _selectedHolders.Contains(holder);

        public IReadOnlyCollection<THolder> GetSelectedHolders()
        {
            return new ReadOnlyCollection<THolder>(_selectedHolders.ToArray());
        }
    }
}