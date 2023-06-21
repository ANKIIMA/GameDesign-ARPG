using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //model of Enemy
    EnemyModel model;

    //necessary components
    private Animator animator;

    private void Awake()
    {
        model = EnemyModel.GetInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animationInitialize();
    }

    #region AnimationInitialize
    private void animationInitialize()
    {
        if(model.DeadFlag == true)
        {
            animator.SetTrigger("DeadFlag");
            model.DeadFlag = false;
        }

        if(model.IsAttacked)
        {
            animator.SetTrigger("IsAttacked");
            model.IsAttacked = false;
        }

    }

    #endregion

    #region damage check
    public void recieveDamage(float damageValue)
    {
        
        if(model.HealthPoint <= 0)
        {
            model.DeadFlag = true;
        }
        else
        {
            model.HealthPoint -= Mathf.Clamp(damageValue, 0f, 100f);
            model.IsAttacked = true;
        }    
        
    }

    #endregion
}
