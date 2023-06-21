using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    //model instance
    private playerModel model;

    //UI
    [SerializeField]
    private Slider HP;                                                                                  //health UI of player
    [SerializeField]
    private Slider MP;                                                                                  //mana UI of player

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
        HP.value = model.PlayerHealthPoint / model.PlayerMaxHealthPoint;
    }

    private void setPlayerManaPoint(float value)
    {
        MP.value = model.PlayerManaPoint / model.PlayerMaxManaPoint;
    }
}
