using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    //model instance
    EnemyModel model;

    //UI
    [SerializeField]
    private Slider HP;


    void Start()
    {
        ////delegate events
        model = EnemyModel.GetInstance();
        model.OnEnemyHealthPointChanged += setHealthPoint;
    }
    
    //Event functions
    private void setHealthPoint(float value)
    {
        HP.value = model.HealthPoint / model.MaxHealthPoint;
    }
}
