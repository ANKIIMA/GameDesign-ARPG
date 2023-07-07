using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public class SelectionStrategyNone<THolder, TData> : SelectionStrategy<THolder, TData>
        where THolder : ItemHolder<TData>
        where TData : class
    {
        public override void ItemClicked(THolder item)
        {
            // Don't select any items!
        }
    }
}