using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yueduWeaponSwitchEvent : MonoBehaviour
{
    [SerializeField] Transform handSword;
    [SerializeField] Transform handGreatSword;

    [SerializeField] Transform backSword;
    [SerializeField] Transform backGreatSword;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">
    /// state==0 ���ô󽣽���С��
    /// state==1 ����С�����ô�</param>
    private void OnWeaponSwitched(int state)
    {
        if(state == 0)
        {
            handSword.gameObject.SetActive(false);
            handGreatSword.gameObject.SetActive(true);

            backSword.gameObject.SetActive(true);
            backGreatSword.gameObject.SetActive(false);
        }

        else if(state == 1)
        {
            handSword.gameObject.SetActive(true);
            handGreatSword.gameObject.SetActive(false);

            backSword.gameObject.SetActive(false);
            backGreatSword.gameObject.SetActive(true);
        }
    }

}
