using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//delegate for player properties
public delegate void OnValueChanged(float val);

public class playerModel
{
    //player properties
    private float playerHealthPoint = 100f;                                                                         //health
    private float playerMaxHealthPoint = 100f;
    private float playerManaPoint = 100f;                                                                           //Skill Point
    private float playerMaxManaPoint = 100f;
    private float manaPointRecovery = 30f;                                                                          //revovery manapoint value of player
    private float damageValue = 10f;                                                                                //damage value of player
    private float attackCost = 50f;                                                                                 //attack will take 20 manapoint
    private float critcalRate = 1.0f;                                                                               //critical rate of attack

    //delegates for properties
    public OnValueChanged OnPlayerHealthPointChanged;
    public OnValueChanged OnPlayerManaPointChanged;

    //Singleton
    private static playerModel model;
    public static playerModel GetInstance()
    {
        if (model == null)
            model = new playerModel();

        return model;
    }

    //Constructor
    private playerModel() { }

    #region get and set for properties
    public float PlayerHealthPoint
    {
        get { return playerHealthPoint; }
        set
        {
            playerHealthPoint = value;
            if(OnPlayerHealthPointChanged != null)
            {
                OnPlayerHealthPointChanged(playerHealthPoint);
            }
        }
    }

    public float PlayerManaPoint
    {
        get { return playerManaPoint; }
        set
        {
            playerManaPoint = value;
            OnPlayerManaPointChanged(playerManaPoint);
        }
    }

    public float PlayerMaxHealthPoint { get => playerMaxHealthPoint; set => playerMaxHealthPoint = value; }
    public float PlayerMaxManaPoint { get => playerMaxManaPoint; set => playerMaxManaPoint = value; }
    public float ManaPointRecovery { get => manaPointRecovery; set => manaPointRecovery = value; }
    public float DamageValue { get => damageValue; set => damageValue = value; }
    public float AttackCost { get => attackCost; set => attackCost = value; }
    public float CritcalRate { get => critcalRate; set => critcalRate = value; }
    #endregion


}
