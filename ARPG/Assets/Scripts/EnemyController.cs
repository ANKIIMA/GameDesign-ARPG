using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //UI
    [SerializeField]
    private Slider HP;

    //
    private bool deadFlag  = false;
    private bool isAttacked = false;

    //necessary components
    private Animator animator;


    //Properties
    private float healthPoint = 100f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        uiInitialize();
    }

    private void uiInitialize()
    {
        animationInitialize();
        HP.value = healthPoint / 100f;
    }

    private void animationInitialize()
    {
        if(deadFlag == true)
        {
            animator.SetTrigger("deadFlag");
            deadFlag = false;
        }

        if(isAttacked)
        {
            animator.SetTrigger("isAttacked");
            isAttacked = false;
        }

    }

    public void recieveDamage(float damageValue)
    {
        
        if(healthPoint <= 0)
        {
            deadFlag = true;
        }
        else
        {
            healthPoint -= Mathf.Clamp(damageValue, 0f, 100f);
            isAttacked = true;
        }    
        
    }
}
