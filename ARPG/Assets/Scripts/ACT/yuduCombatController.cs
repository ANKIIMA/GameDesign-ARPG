using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Combat;

public class yuduCombatController : BasicCombatModel
{
    //Speed
    [SerializeField, Header("AnimationRootAttackScale"), Range(.1f, 10f)]
    private float animationRootAttackScale;

    //Detectoin
    [SerializeField, Header("Enemy Detection")] private Transform detectionCenter;
    [SerializeField] private float detectionRadius;

    //Weapon
    [SerializeField] private int weaponIndex;


    //Buffer
    private Collider[] detectionedTarget = new Collider[1];

    private void Update()
    {
        UpdateAttackAnimation();
        DetectionTarget();
        ActionMotion();
    }

    private void UpdateAttackAnimation()
    {
        if (InputController.LAtk)
        {
            animator.SetTrigger(lAtkID);


        }
        
        
    }






    private void ActionMotion()
    {
        if (animator.CheckAnimationTag("Attack"))
        {
            MovementBase.MoveInterface(transform.forward, animator.GetFloat(animationMoveID) * animationRootAttackScale, true);
        }
    }

    #region �������

    /// <summary>
    /// ����״̬�Ƿ������Զ���������
    /// </summary>
    /// <returns></returns>
    private bool CanAttackLockOn()
    {
        if (animator.CheckAnimationTag("Attack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                return true;
            }
        }
        return false;
    }


    private void DetectionTarget()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRadius, detectionedTarget,
            EnemyLayerMask);

        //�������ܲ���
    }

    #endregion
}
