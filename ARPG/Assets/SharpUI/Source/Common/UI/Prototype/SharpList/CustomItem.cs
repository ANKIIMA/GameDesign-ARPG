using SharpUI.Source.Common.UI.Elements.List.ListItem;
using TMPro;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Prototype.SharpList
{
    [RequireComponent(typeof(Item))]
    public class CustomItem : Item
    {
        [SerializeField] public TMP_Text leftText;
        [SerializeField] public TMP_Text rightText;
    }
}