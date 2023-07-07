using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.List.ListItem
{
    [RequireComponent(typeof(Item))]
    public class ItemImage : Item
    {
        [SerializeField] public TMP_Text textTitle;
        [SerializeField] public TMP_Text textDescription;
        [SerializeField] public Image iconBackground;
        [SerializeField] public Image imageIcon;
    }
}