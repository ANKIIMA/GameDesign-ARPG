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

    [Header("文本")]
    [SerializeField] private TextAsset textFile;
    [SerializeField] private int index;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        GetTextFromFile(textFile);
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void UpdateDialogueText()
    {
        if(index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }
        dialogueText.text = textList[index];
        index++;
    }
}
