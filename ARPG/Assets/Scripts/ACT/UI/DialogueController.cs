using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    //ref
    [Header("UI组件")]
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image characterImage;

    [Header("文本显示")]
    [SerializeField] private DialogueSO dialogueso;
    [SerializeField] private TextAsset textFile;
    [SerializeField] private int index;
    [SerializeField] private float textSpeed;

    [Header("头像")]
    [SerializeField] private Sprite yueduFace;
    [SerializeField] private Sprite mllFace;

    [Header("任务进度")]
    [SerializeField] private int currentState;

    List<string> textList = new List<string>();
    private bool textFinished;

    public bool TextFinished { get => textFinished; set => textFinished = value; }




    #region Unity事件函数
    // Start is called before the first frame update
    void Awake()
    {
        currentState = 0;
        textFile = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        textFinished = true;
        if(currentState == 0)
        {
            textFile = dialogueso.GetTextAsset("Start");
        }
        else if(currentState == 1)
        {
            textFile = dialogueso.GetTextAsset("End");
        }
        GetTextFromFile(textFile);
    }

    #endregion


    #region 内部方法
    private void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split('\n');

        foreach(var line in lineDate)
        {
            textList.Add(line);
        }
    }

    private IEnumerator SetTextUI()
    {
        textFinished = false;
        dialogueText.text = "";

        switch(textList[index].Trim().ToString())
        {
            case "A":
                characterImage.sprite = mllFace;
                index++;
                break;
            case "B":
                characterImage.sprite = yueduFace;
                index++;
                break;
        }

        for(int i = 0; i < textList[index].Length; i++)
        {
            dialogueText.text += textList[index][i];
            yield return new WaitForSeconds(textSpeed);
        }
        index++;
        textFinished = true;
    }

    #endregion


    #region 外部方法
    public void UpdateDialogueText()
    {
        if(index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            currentState = 1;
            return;
        }
        if(textList.Count == 0)
        {
            return;
        }
        if (textFinished == false) return;
        StartCoroutine(SetTextUI());
    }

    public void InteruptDialogue()
    {
        index = 0;
        gameObject.SetActive(false);
    }
    #endregion
}
