using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.UI;

namespace ACT.Interaction {
    public class BasicInteractionModel : MonoBehaviour
    {
        //ref
        [SerializeField] protected Transform currentInteractionTarget;



        //Detection
        [SerializeField] protected Transform interactionDetectionCenter;
        [SerializeField] protected float interactionDetectionRadius;

        //Layer Mask
        [SerializeField] protected LayerMask interativeLayerMask;
        [SerializeField] protected LayerMask obstacleLayerMask;

        [SerializeField] private float viewFieldCosValue;

        //private
        private Collider[] interactiveTargetColliders = new Collider[1];
        private BasicUIRefModel basicUIRefModel;

        #region Unity�¼�����
        public virtual void Start()
        {
            basicUIRefModel = GetComponent<BasicUIRefModel>();
        }

        public virtual void Update()
        {
            InteractionDetection();
        }

        #endregion


        #region �ڲ�����
        //�ж��Ƿ���Խ��н���
        private bool IsAbleToInteraction(Transform target)
        {
            //û���ϰ���
            if (Physics.Raycast(transform.root.position + 0.5f * Vector3.up, target.root.position - transform.root.position,
                out var raycastHit, interactionDetectionRadius, obstacleLayerMask) == false)
            {
                //��NPCǰ��������
                if (Vector3.Dot((target.position - transform.root.position).normalized, transform.root.forward) > viewFieldCosValue)
                {
                    //����С������
                    if(Vector3.Distance(transform.root.position, target.root.transform.position) < interactionDetectionRadius)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ��⽻������
        /// </summary>
        /// <returns></returns>
        private void InteractionDetection()
        {
            //��⵽��ɫ
            int targetCount = Physics.OverlapSphereNonAlloc(interactionDetectionCenter.position, interactionDetectionRadius, interactiveTargetColliders, interativeLayerMask);
            if (targetCount > 0)
            {
                //�Ƿ��ܽ���
                if (IsAbleToInteraction(interactiveTargetColliders[0].transform.root.transform) == true)
                {
                    //����� Ҫô�����Ի���ʾҪô�����Ի���
                    //���öԻ�����
                    SetCurrentInteractionTarget(interactiveTargetColliders[0].transform.root.transform);
                    if(basicUIRefModel.uiManagementRef.DialoguePanel.activeSelf == false)
                    {
                        basicUIRefModel.uiManagementRef.OnEnableInteractionInfoPanel();
                    }
                    else
                    {
                        basicUIRefModel.uiManagementRef.OnDisableInteractionInfoPanel();
                    }

                }
                else
                {
                    //���ܽ�����UIȫ��
                    SetCurrentInteractionTarget(null);
                    basicUIRefModel.uiManagementRef.OnDisableInteractionInfoPanel();
                    basicUIRefModel.uiManagementRef.OnDisableDialoguePanel();
                }
            }

        }

        /// <summary>
        /// ���õ�ǰ��������
        /// </summary>
        /// <param name="target">Ŀ�����</param>
        private void SetCurrentInteractionTarget(Transform target) 
        {
            if(currentInteractionTarget == null || currentInteractionTarget != target)
            {
                currentInteractionTarget = target;
            }
        }


        #endregion

        #region �ⲿ����

        #endregion
    }
}

