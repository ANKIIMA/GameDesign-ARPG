using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Factory;
using SharpUI.Source.Data.Model.Character;

namespace SharpUI.Source.Client.UI.User.CharacterSelect.CharacterList
{
    public class CharacterItemHolder : ItemHolder<Character>
    {
        private readonly ItemImage _itemImage;
        private readonly CharacterIconFactory _characterIconFactory;

        public CharacterItemHolder(ItemImage item, CharacterIconFactory characterIconFactory) : base(item)
        {
            _itemImage = item;
            _characterIconFactory = characterIconFactory;
        }

        public override void BindData(Character itemData)
        {
            base.BindData(itemData);
            RenderItem();
        }
        
        private void RenderItem()
        {
            _itemImage.textTitle.text = data.Name;
            _itemImage.textDescription.text = $"Level {data.Level}";
            _itemImage.imageIcon.sprite = _characterIconFactory.CreateSpriteFor(data.ClassType);
        }
    }
}