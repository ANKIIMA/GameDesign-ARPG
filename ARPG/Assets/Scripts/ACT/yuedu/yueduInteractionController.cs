using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.UI;

public class yueduInteractionController : MonoBehaviour
{
    private PlayerInputController inputController;
    private BasicUIRefModel basicUIRefModel;
    private DialogueController dialogueController;

    private int state;
    private void Start()
    {
        inputController = GetComponent<PlayerInputController>();
        basicUIRefModel = GetComponentInParent<BasicUIRefModel>();
        dialogueController = basicUIRefModel.uiManagementRef.DialoguePanel.GetComponent<DialogueController>();
    }

    private void Update()
    {
        CheckDialogue();
        NextDialogue();
        EnableInventory();
    }

    /// <summary>
    /// 检查对话框是否开启
    /// </summary>
    private void CheckDialogue()
    {
        //开启对话框
        if(inputController.Interact)
        {
            basicUIRefModel.uiManagementRef.OnEnableDialoguePanel();
            basicUIRefModel.uiManagementRef.OnDisableInteractionInfoPanel();
        }
    }

    /// <summary>
    /// 下一段对话
    /// </summary>
    private void NextDialogue()
    {
        if(inputController.Interact)
        {
            dialogueController.UpdateDialogueText();
        }
    }

    private void EnableInventory()
    {
        if(inputController.Inventory)
        {
            basicUIRefModel.uiManagementRef.OnSwitchInventory();
        }
    }

    
}
