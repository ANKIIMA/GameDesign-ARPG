using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIPrefab : MonoBehaviour
{
    [SerializeField] public ItemSO itemso;
    [SerializeField] public Image image;
    [SerializeField] public Text num;
    [SerializeField] public GameObject itemDiscriptionPanel;
    [SerializeField] public GameObject player;


    public void OnItemInfoClick()
    {
        InventoryController.UpdateItemInfo(itemso.ItemInfo);
        itemDiscriptionPanel.SetActive(!itemDiscriptionPanel.activeSelf);
    }

    public void OnItemIconClick()
    {
        if(itemso.itemType == ItemType.weapon)
        {
            player.GetComponent<yueduWeaponSwitchEvent>().OnEquipGreatSwordEvent();
            itemso.ItemHeld = 0;
        }
        if(itemso.itemType == ItemType.portion)
        {
            player.GetComponent<yueduHealthController>().TakePortion(itemso.ItemName);
            itemso.ItemHeld -= 1;
            if (itemso.ItemHeld < 0) itemso.ItemHeld = 0;
        }

        InventoryController.UpdateItemInfo();
    }
}
