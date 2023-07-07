using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public class SelectionStrategyAll<THolder, TData> : SelectionStrategy<THolder, TData>
        where THolder : ItemHolder<TData>
        where TData : class
    {
        public SelectionStrategyAll() : base(ItemSelectionType.All, int.MaxValue)
        {
        }

        public override void ItemClicked(THolder item)
        {
            if (IsSelected(item))
            {
                DeselectItem(item);
            }
            else
            {
                SelectItem(item);
            }
        }
    }
}