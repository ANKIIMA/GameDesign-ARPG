using System.Collections.Generic;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public interface ISelectionStrategy<THolder>
    {
        ItemSelectionType GetSelectionType();
        int GetMaxAmount();
        void DeselectAll();
        void DeselectItem(THolder holder);
        void ItemClicked(THolder holder);
        bool HasSelections();
        ISelectionChangeListener<TData> GetSelectionChangeListener<TData>() where TData : class;
        IReadOnlyCollection<THolder> GetSelectedHolders();
    }
}