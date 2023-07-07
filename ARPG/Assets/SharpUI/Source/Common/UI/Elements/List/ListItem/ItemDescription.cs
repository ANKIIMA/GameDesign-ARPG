using TMPro;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.List.ListItem
{
    [RequireComponent(typeof(Item))]
    public class ItemDescription : Item
    {
        [SerializeField] public TMP_Text textTitle;
        [SerializeField] public TMP_Text textDescription;
    }
}