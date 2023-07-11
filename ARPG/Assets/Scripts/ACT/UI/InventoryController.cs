using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    static InventoryController instance;

    //背包数据
    [SerializeField] private InventorySO inventory;
    //背包网格
    [SerializeField] private GameObject grid;
    //物品预制件
    [SerializeField] private ItemUIPrefab itemPrefab;
    //物品描述
    [SerializeField] private Text itemDiscription;
    //描述框
    [SerializeField] private GameObject itemDiscriptionPanel;
    [SerializeField] private GameObject yuedu;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        UpdateItemInfo();
        instance.itemDiscription.text = "";
    }

    public static void UpdateItemInfo(string itemDis)
    {
        instance.itemDiscription.text = itemDis;
    }

    /// <summary>
    /// 创建物品实例，并将itemso提供给实例读取数据和赋值
    /// </summary>
    /// <param name="itemso">持久化的物品数据</param>
    public static void CreateNewItem(ItemSO itemso)
    {
        ItemUIPrefab newItem = Instantiate(instance.itemPrefab, instance.grid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.grid.transform);

        newItem.itemso = itemso;
        newItem.image.sprite = itemso.ItemImage;
        newItem.num.text = itemso.ItemHeld.ToString();
        newItem.itemDiscriptionPanel = instance.itemDiscriptionPanel;
        newItem.player = instance.yuedu;
    }

    public static void UpdateItemInfo()
    {
        for(int i = 0; i < instance.grid.transform.childCount; i++)
        {
            if (instance.grid.transform.childCount == 0) break;

            Destroy(instance.grid.transform.GetChild(i).gameObject);

        }

        for(int i = 0; i < instance.inventory.ItemList.Count; i++)
        {
            if (instance.inventory.ItemList[i].ItemHeld <= 0) continue;
            CreateNewItem(instance.inventory.ItemList[i]);
        }
    }

}
