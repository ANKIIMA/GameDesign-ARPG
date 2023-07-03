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
    [SerializeField] private bool weaponIndex;


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
        //Left Attack
        if (InputController.LAtk)
        {
            animator.SetTrigger(lAtkID);
        }

        //Equip Secondary Weapon
        if(InputController.WeaponSwitch)
        {
            if(weaponIndex == false)
            {
                weaponIndex = true;
            }
            else
            {
                weaponIndex = false;
            }
        }
        animator.SetBool(sWeaponID, weaponIndex);
    }






    private void ActionMotion()
    {
        if (animator.CheckAnimationTag("Attack"))
        {
            MovementBase.MoveInterface(transform.forward, animator.GetFloat(animationMoveID) * animationRootAttackScale, true);
        }
    }

    #region 动作检测

    /// <summary>
    /// 攻击状态是否允许自动锁定敌人
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

        //后续功能补充
    }

    #endregion
}
