using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private int itemHeld;
    [SerializeField] private string itemInfo;

    public string ItemName { get => itemName; set => itemName = value; }
    public Sprite ItemImage { get => itemImage; set => itemImage = value; }
    public int ItemHeld { get => itemHeld; set => itemHeld = value; }
    public string ItemInfo { get => itemInfo; set => itemInfo = value; }
}
