using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManagement : MonoBehaviour
{
    //Layer ID
    private int playerLayerID;
    private int enemyLayerID;

    //ref
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private GameObject InteractionInfoPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject inventoryPanel;

    private bool inventoryIsEnable;

    public Image PlayerHealthBar { get => playerHealthBar; set => playerHealthBar = value; }
    public Image EnemyHealthBar { get => enemyHealthBar; set => enemyHealthBar = value; }
    public GameObject InteractionInfoPanel1 { get => InteractionInfoPanel; set => InteractionInfoPanel = value; }
    public GameObject DialoguePanel { get => dialoguePanel; set => dialoguePanel = value; }
    public GameObject Inventory { get => inventoryPanel; set => inventoryPanel = value; }
    public bool InventoryIsEnable { get => inventoryIsEnable; set => inventoryIsEnable = value; }

    #region Unity事件函数

    private void Start()
    {
        playerLayerID = LayerMask.NameToLayer("Player");
        enemyLayerID = LayerMask.NameToLayer("Enemy");
        InventoryIsEnable = Inventory.activeSelf;
    }



    #endregion

    #region 外部方法

    public void OnButtonStart()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void OnButtonExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnPlayerHealthValueChange(float value)
    {
        playerHealthBar.fillAmount = value;
    }

    public void OnEnemyHealthValueChange(float value)
    {
        enemyHealthBar.fillAmount = value;
    }

    public void OnEnableInteractionInfoPanel()
    {
        InteractionInfoPanel.SetActive(true);
    }
    
    public void OnDisableInteractionInfoPanel()
    {
        InteractionInfoPanel.SetActive(false);
    }

    public void OnEnableDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }
    
    public void OnDisableDialoguePanel()
    {
        dialoguePanel.GetComponent<DialogueController>().InteruptDialogue();
        dialoguePanel.SetActive(false);
    }

    public void OnSwitchInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        InventoryIsEnable = !InventoryIsEnable;
    }

    #endregion
}
