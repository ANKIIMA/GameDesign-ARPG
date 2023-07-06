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

    #region Unity�¼�����
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

    #region �ڲ�����

    private void AIView()
    {
        //������
        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRadius, colliderTarget, PlayerMaskLayer);

        //��⵽���
        if(targetCount > 0)
        {
            //��������ҵ����߼���Ƿ�����ϰ��� 
            if (Physics.Raycast((transform.root.position + transform.root.up * 0.5f), 
                (colliderTarget[0].transform.position - transform.root.position).normalized, out var hitInfo, detectionRadius, ObstacleLayer)
                 == false) 
            {
                //�ж����ߺ�ǰ������ļн�����ֵ�Ƿ����0.15(80��)
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

    #region �ⲿ����

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
