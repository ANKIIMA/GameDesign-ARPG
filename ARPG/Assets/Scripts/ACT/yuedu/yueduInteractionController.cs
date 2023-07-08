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
    }

    /// <summary>
    /// ���Ի����Ƿ���
    /// </summary>
    private void CheckDialogue()
    {
        //�����Ի���
        if(inputController.Interact)
        {
            basicUIRefModel.uiManagementRef.OnEnableDialoguePanel();
            basicUIRefModel.uiManagementRef.OnDisableInteractionInfoPanel();
        }
    }

    /// <summary>
    /// ��һ�ζԻ�
    /// </summary>
    private void NextDialogue()
    {
        if(inputController.Interact && basicUIRefModel.uiManagementRef.DialoguePanel.activeSelf == true)
        {
            dialogueController.UpdateDialogueText();
        }
    }
}
