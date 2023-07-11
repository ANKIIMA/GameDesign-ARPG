using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    static InventoryController instance;

    //��������
    [SerializeField] private InventorySO inventory;
    //��������
    [SerializeField] private GameObject grid;
    //��ƷԤ�Ƽ�
    [SerializeField] private ItemUIPrefab itemPrefab;
    //��Ʒ����
    [SerializeField] private Text itemDiscription;
    //������
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
    /// ������Ʒʵ��������itemso�ṩ��ʵ����ȡ���ݺ͸�ֵ
    /// </summary>
    /// <param name="itemso">�־û�����Ʒ����</param>
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
