using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Elements.List.Selection
{
    public class SelectionStrategySingle<THolder, TData> : SelectionStrategy<THolder, TData>
        where THolder : ItemHolder<TData>
        where TData : class
    {
        public SelectionStrategySingle() : base(ItemSelectionType.Single, 1)
        {
        }

        public override void ItemClicked(THolder item)
        {
            DeselectAll();
            SelectItem(item);
        }
    }
}