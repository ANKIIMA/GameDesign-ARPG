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

    #region �ⲿ����
    private void Update()
    {
        AIView();
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




    #endregion



}
