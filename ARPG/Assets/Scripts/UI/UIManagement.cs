using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagement : MonoBehaviour
{




    #region �ⲿ����

    public void OnButtonStart()
    {
        SceneManager.LoadScene("Scene2");
    }

    public void OnButtonExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }




    #endregion
}
