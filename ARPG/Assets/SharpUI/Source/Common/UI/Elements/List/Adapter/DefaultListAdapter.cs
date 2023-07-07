using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Factory;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.List.Adapter
{
    public class DefaultListAdapter : ListAdapter<string>
    {
        [SerializeField] public ListItemFactory itemFactory;

        public void SetItemsAndNotify(List<string> items)
        {
            SetData(items);
            NotifyDataSetChanged();
        }
        
        protected override ItemHolder<string> CreateListItemHolder()
        {
            var item = itemFactory.CreateListItemText();
            var holder = new DefaultItemHolder(item);
            return holder;
        }

        protected override void BindListItemHolder(ItemHolder<string> holder, int position)
        {
            (holder as DefaultItemHolder)?.BindData(GetData(position));
        }
    }
}