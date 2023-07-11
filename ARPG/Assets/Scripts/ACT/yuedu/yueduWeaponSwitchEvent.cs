using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yueduWeaponSwitchEvent : MonoBehaviour
{
    [SerializeField] Transform handSword;
    [SerializeField] Transform handGreatSword;

    [SerializeField] Transform backSword;
    [SerializeField] Transform backGreatSword;

    [SerializeField] private bool hasGreatSword;

    public bool HasGreatSword { get => hasGreatSword; set => hasGreatSword = value; }

    private void Start()
    {
        hasGreatSword = false;
        InitWeaponState();
    }

    private void InitWeaponState()
    {
        handSword.gameObject.SetActive(true);
        handGreatSword.gameObject.SetActive(false);

        backSword.gameObject.SetActive(false);
        backGreatSword.gameObject.SetActive(false);
    }


    /// <summary>
    /// �л�����
    /// </summary>
    /// <param name="state">
    /// state==0 ���ô󽣽���С��
    /// state==1 ����С�����ô�</param>
    private void OnWeaponSwitched(int state)
    {
        if(hasGreatSword == false)
        {
            return;
        }

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

    public void OnEquipGreatSwordEvent()
    {
        hasGreatSword = true;
        backGreatSword.gameObject.SetActive(true);

    }

}
