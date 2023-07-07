using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public class SelectionStrategyLimited<THolder, TData> : SelectionStrategy<THolder, TData>
        where THolder : ItemHolder<TData>
        where TData : class
    {
        public SelectionStrategyLimited(int amount) : base(ItemSelectionType.Limited, amount)
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
                if (CanSelect())
                    SelectItem(item);
            }
        }
    }
}