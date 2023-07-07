using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Prototype.SharpList;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Factory
{
    public class ListItemFactory : MonoBehaviour
    {
        [SerializeField] public GameObject listItemPrefab;
        [SerializeField] public GameObject listItemDescriptionPrefab;
        [SerializeField] public GameObject listItemImagePrefab;
        [SerializeField] public GameObject listItemCustom;

        public ItemText CreateListItemText()
            => Instantiate(listItemPrefab).GetComponent<ItemText>();
        
        public ItemDescription CreateListItemDescription()
            => Instantiate(listItemDescriptionPrefab).GetComponent<ItemDescription>();
        
        public ItemImage CreateListItemImage()
            => Instantiate(listItemImagePrefab).GetComponent<ItemImage>();

        public CustomItem CreateCustomListItem()
            => Instantiate(listItemCustom).GetComponent<CustomItem>();
    }
}