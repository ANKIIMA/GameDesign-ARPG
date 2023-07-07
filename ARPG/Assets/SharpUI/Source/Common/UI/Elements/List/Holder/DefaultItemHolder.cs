using SharpUI.Source.Common.UI.Elements.List.ListItem;

namespace SharpUI.Source.Common.UI.Elements.List.Holder
{
    public class DefaultItemHolder : ItemHolder<string>
    {
        private readonly ItemText _itemText;
        
        public DefaultItemHolder(ItemText itemText) : base(itemText)
        {
            _itemText = itemText;
        }

        public override void BindData(string text)
        {
            base.BindData(text);
            _itemText.textTitle.text = text;
        }
    }
}