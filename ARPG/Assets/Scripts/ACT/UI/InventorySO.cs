using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory", menuName ="Inventory/New Inventory")]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<ItemSO> itemList = new List<ItemSO>();

    public List<ItemSO> ItemList { get => itemList; set => itemList = value; }
}
