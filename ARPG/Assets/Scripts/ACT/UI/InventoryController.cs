using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    static InventoryController instance;

    [SerializeField] private InventorySO inventory;
    [SerializeField] private GameObject grid;
    [SerializeField] private ItemUIPrefab itemPrefab;
    [SerializeField] private Text itemDiscription;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private static void CreateNewItem(ItemSO item)
    {
        ItemUIPrefab newItem = Instantiate(instance.itemPrefab, instance.grid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.grid.transform);
    }
}
