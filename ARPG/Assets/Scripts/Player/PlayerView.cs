using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuloGames.UI;

public class PlayerView : MonoBehaviour
{
    //model instance
    private playerModel model;

    //UI
    [SerializeField] private UIProgressBar m_HpProgressBar;                                             //health UI of player
    [SerializeField] private UIProgressBar m_MpProgressBar;                                             //mana UI of player

    private void Start()
    {
        //delegate events
        model = playerModel.GetInstance();
        model.OnPlayerHealthPointChanged += setPlayerHealthPoint;
        model.OnPlayerManaPointChanged += setPlayerManaPoint;
    }

    //enents functions

    private void setPlayerHealthPoint(float value)
    {
        if (this.m_HpProgressBar == null)
            return;

        this.m_HpProgressBar.fillAmount = model.PlayerHealthPoint / model.PlayerMaxHealthPoint;
    }

    private void setPlayerManaPoint(float value)
    {
        if (this.m_MpProgressBar == null)
            return;

        this.m_MpProgressBar.fillAmount = model.PlayerManaPoint / model.PlayerMaxManaPoint;
    }
}
