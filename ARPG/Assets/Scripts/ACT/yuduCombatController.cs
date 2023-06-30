using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Combat;

public class yuduCombatController : BasicCombatModel
{
    //Speed
    [SerializeField, Header("攻击移动速度倍率"), Range(.1f, 10f)]
    private float attackMoveMult;

    //检测
    [SerializeField, Header("检测敌人")] private Transform detectionCenter;
    [SerializeField] private float detectionRang;

    //缓存
    private Collider[] detectionedTarget = new Collider[1];

    private void Update()
    {
        PlayerAttackAction();
        DetectionTarget();
        ActionMotion();
    }

    private void PlayerAttackAction()
    {
        if (InputController.LAtk)
        {
            _animator.SetTrigger(lAtkID);

        }
    }






    private void ActionMotion()
    {
        if (_animator.CheckAnimationTag("Attack"))
        {
            MovementBase.MoveInterface(transform.forward, _animator.GetFloat(animationMoveID) * attackMoveMult, true);
        }
    }

    #region 动作检测

    /// <summary>
    /// 攻击状态是否允许自动锁定敌人
    /// </summary>
    /// <returns></returns>
    private bool CanAttackLockOn()
    {
        if (_animator.CheckAnimationTag("Attack"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                return true;
            }
        }
        return false;
    }


    private void DetectionTarget()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRang, detectionedTarget,
            enemyLayer);

        //后续功能补充
    }

    #endregion
}
