using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Factory;
using SharpUI.Source.Data.Model.Character;
using UnityEngine;

namespace SharpUI.Source.Client.UI.User.CharacterSelect.CharacterList
{
    public class CharacterListAdapter : ListAdapter<Character>
    {
        [SerializeField] public ListItemFactory itemFactory;
        [SerializeField] public CharacterIconFactory characterIconFactory;

        public void SetCharactersAndNotify(List<Character> characters)
        {
            SetData(characters);
            NotifyDataSetChanged();
        }

        protected override ItemHolder<Character> CreateListItemHolder()
        {
            var item = itemFactory.CreateListItemImage();
            var holder = new CharacterItemHolder(item, characterIconFactory);
            return holder;
        }

        protected override void BindListItemHolder(ItemHolder<Character> holder, int position)
        {
            (holder as CharacterItemHolder)?.BindData(GetData(position));
        }
    }
}