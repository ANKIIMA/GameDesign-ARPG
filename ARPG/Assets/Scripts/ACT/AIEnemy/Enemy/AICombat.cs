using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Combat;

public class AICombat : BasicCombatModel
{

    [SerializeField, Header("Target Detection")] private Transform detectionCenter;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float viewFieldCosValue;
    [SerializeField] private LayerMask PlayerMaskLayer;
    [SerializeField] private LayerMask ObstacleLayer;

    Collider[] colliderTarget = new Collider[1];

    [SerializeField, Header("Current Target")] private Transform currentTarget;

    public Transform CurrentTarget { get => currentTarget; set => currentTarget = value; }

    #region Unity事件函数
    private void Update()
    {
        AIView();
        LockOnTarget();
        UpdateAnimationMove();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(detectionCenter.position, detectionRadius);
    }


    #endregion

    #region 内部函数

    private void AIView()
    {
        //检测玩家
        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRadius, colliderTarget, PlayerMaskLayer);

        //检测到玩家
        if(targetCount > 0)
        {
            //从自身到玩家的射线检测是否存在障碍物 
            if (Physics.Raycast((transform.root.position + transform.root.up * 0.5f), 
                (colliderTarget[0].transform.position - transform.root.position).normalized, out var hitInfo, detectionRadius, ObstacleLayer)
                 == false) 
            {
                //判断连线和前方方向的夹角余弦值是否大于0.15(80度)
                if(Vector3.Dot((colliderTarget[0].transform.position - transform.root.position).normalized, transform.root.forward) > viewFieldCosValue)
                {
                    currentTarget = colliderTarget[0].transform;
                }
            }
        }
    }

    private void LockOnTarget()
    {
        if (currentTarget != null && animator.CheckAnimationTag("NormalMotion"))
        {
            animator.SetFloat(lockOnID, 1f);
            transform.rotation = transform.LockOnTarget(currentTarget, transform.root.transform, 100f);
        }
        else
        {
            animator.SetFloat(lockOnID, 0f);
        }
    }

    private void UpdateAnimationMove()
    {
        if(animator.CheckAnimationTag("Slide") == true)
        {
            MovementBase.MoveInterface(transform.root.transform.forward, animator.GetFloat(animationMoveID), true);
        }
    }




    #endregion

    #region 外部函数

    public Transform GetCurrentTarget()
    {
        if(currentTarget == null)
        {
            return null;
        }

        return currentTarget;
    }

    public float GetCurrentTargetDistance() => Vector3.Distance(currentTarget.position, transform.root.position);
    public Vector3 GetDirectionToTarget() => (currentTarget.position - transform.root.position).normalized;
    #endregion

}
