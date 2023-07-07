using SharpUI.Source.Common.UI.Elements.List.ListItem;

namespace SharpUI.Source.Common.UI.Elements.List.Holder
{
    public class ItemHolder<TData> where TData : class
    {
        public readonly Item item;

        protected TData data;

        public ItemHolder(Item item)
        {
            this.item = item;
        }

        public virtual void BindData(TData itemData) => data = itemData;

        public TData GetData() => data;
    }
}