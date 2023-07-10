using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldController : MonoBehaviour
{
    [SerializeField] private  ItemSO item;
    [SerializeField] private InventorySO inventory;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //AddNewItem();
            other.GetComponent<yueduWeaponSwitchEvent>().EquipGreatSword();
            Destroy(gameObject);
        }
    }

    private void AddNewItem()
    {
        if(inventory.ItemList.Contains(item) == false)
        {
            inventory.ItemList.Add(item);
            Destroy(gameObject);
        }
        else
        {
            item.ItemHeld += 1;
        }
    }
}
