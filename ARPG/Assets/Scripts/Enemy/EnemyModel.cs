using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    //Singleton
    private static EnemyModel model;

    //flags for animator
    private bool deadFlag = false;
    private bool isAttacked = false;

    //Properties
    private float healthPoint = 100f;
    private float maxHealthPoint = 100f;

    //delegates for properties
    public OnValueChanged OnEnemyHealthPointChanged;
    public static EnemyModel GetInstance()
    {
        if(model == null)
        {
            model = new EnemyModel();
        }
        return model;
    }

    #region get and set of properties
    public bool DeadFlag { get => deadFlag; set => deadFlag = value; }
    public bool IsAttacked { get => isAttacked; set => isAttacked = value; }
    public float MaxHealthPoint { get => maxHealthPoint; set => maxHealthPoint = value; }
    public float HealthPoint 
    {
        get { return healthPoint; }
        set
        {
            healthPoint = value;
            if(OnEnemyHealthPointChanged != null)
            {
                OnEnemyHealthPointChanged(healthPoint);
            }
        }

    }
    #endregion

}
