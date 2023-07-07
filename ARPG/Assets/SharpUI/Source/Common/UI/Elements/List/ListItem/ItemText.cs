using TMPro;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.List.ListItem
{
    [RequireComponent(typeof(Item))]
    public class ItemText : Item
    {
        [SerializeField] public TMP_Text textTitle;
    }
}