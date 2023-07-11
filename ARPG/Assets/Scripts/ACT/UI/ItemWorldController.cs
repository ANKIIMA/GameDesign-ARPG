using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldController : MonoBehaviour
{
    [SerializeField] private  ItemSO itemso;
    [SerializeField] private InventorySO inventoryso;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    private void AddNewItem()
    {
        
        //�����в����ڸ�������
        if(inventoryso.ItemList.Contains(itemso) == false)
        {
            inventoryso.ItemList.Add(itemso);
        }
        //���ڸ�������
        else
        {
            itemso.ItemHeld += 1;

        }

        InventoryController.UpdateItemInfo();
    }


}
