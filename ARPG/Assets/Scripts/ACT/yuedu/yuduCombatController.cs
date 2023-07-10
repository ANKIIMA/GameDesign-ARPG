using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Combat;

public class yuduCombatController : BasicCombatModel
{
    //ref
    [SerializeField] private Transform currentTarget;

    //Speed
    [SerializeField, Header("AnimationRootAttackScale"), Range(.1f, 10f)]
    private float animationRootAttackScale;

    //Detectoin
    [SerializeField, Header("Enemy LockOn Detection")] private Transform lockOnDetectionCenter;
    [SerializeField] private float detectionRadius;

    //Weapon
    [SerializeField] private bool weaponIndex;


    //Buffer
    private Collider[] detectionedTarget = new Collider[1];

    #region Unity事件函数
    private void Update()
    {
        UpdateAttackAnimation();
        ActionMotion();
        DetectionTarget();
        UpdateCurrentTarget();
    }

    private void LateUpdate()
    {
        OnAttackAutoLockOn();
    }
    #endregion


    #region 内部函数

    /// <summary>
    /// 判断是否能够输入攻击
    /// </summary>
    /// <returns></returns>
    private bool CanInputAttack()
    {
        //bug: 不足0.25的标准化时间也会发生状态转移？
        if (animator.CheckCurrentAnimationTagTimeIsGreater("Attack", 0.22f))
        {
            return true;
        }
        if (animator.CheckCurrentAnimationTagTimeIsGreater("GSAttack", 0.30f))
        {
            return true;
        }

        if (animator.CheckAnimationTag("NormalMotion") || animator.CheckAnimationTag("GSMotion")) return true;


        //已解决 Trigger未重置 导致状态机的Trigger和理想情况不一致
        //原本返回false后经过多次点击Trigger实际值仍然是true
        animator.ResetTrigger(lAtkID);
        animator.ResetTrigger(rAtkID);
        return false;
    }


    /// <summary>
    /// 更新攻击动画
    /// </summary>
    private void UpdateAttackAnimation()
    {
        if (CanInputAttack() == false)
        {
            return;
        }
        //Left Attack
        if (InputController.LAtk)
        {
            animator.SetTrigger(lAtkID);
        }

        //Equip Secondary Weapon
        if (InputController.WeaponSwitch)
        {
            if (GetComponent<yueduWeaponSwitchEvent>().HasGreatSword == false) return;

            if (weaponIndex == false)
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


    /// <summary>
    /// 控制攻击时的移动
    /// </summary>
    private void ActionMotion()
    {
        if (animator.CheckAnimationTag("Attack") || animator.CheckAnimationTag("GSAttack"))
        {
            MovementBase.MoveInterface(transform.forward, animator.GetFloat(animationMoveID) * animationRootAttackScale, true);
        }
    }


    #region 攻击自动索敌
    /// <summary>
    /// 旋转到锁定对象
    /// </summary>
    private void OnAttackAutoLockOn()
    {
        if(CanAttackLockOn())
        {
            if (animator.CheckAnimationTag("Attack") || animator.CheckAnimationTag("GSAttack"))
            {
                transform.root.transform.rotation = transform.LockOnTarget(currentTarget, transform.root.transform, 50f);
            }
        }
    }



    /// <summary>
    /// 判断攻击状态是否允许自动锁定敌人
    /// </summary>
    /// <returns></returns>
    private bool CanAttackLockOn()
    {
        if (animator.CheckAnimationTag("Attack") || animator.CheckAnimationTag("GSAttack"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 检测前方敌人
    /// </summary>
    private void DetectionTarget()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(lockOnDetectionCenter.position, detectionRadius, detectionedTarget,
            EnemyLayerMask);

        if(targetCount > 0)
        {
            SetCurrentTarget(detectionedTarget[0].transform);
        }
        
    }

    /// <summary>
    /// 设置当前锁定目标
    /// </summary>
    /// <param name="target">锁定对象</param>
    private void SetCurrentTarget(Transform target)
    {
        if(currentTarget == null || currentTarget != target)
        {
            currentTarget = target;
        }
    }

    /// <summary>
    /// 更新锁定状态
    /// 移动时不能自动锁定
    /// </summary>
    private void UpdateCurrentTarget()
    {
        if(animator.CheckAnimationTag("NormalMotion") || animator.CheckAnimationTag("GSMotion"))
        {
            if(InputController.Movement.sqrMagnitude > 0)
            {
                currentTarget = null;
            }
        }
    }
    #endregion

    #endregion
}
