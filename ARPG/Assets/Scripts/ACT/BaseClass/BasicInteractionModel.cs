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

        #region Unity事件函数
        public virtual void Start()
        {
            basicUIRefModel = GetComponent<BasicUIRefModel>();
        }

        public virtual void Update()
        {
            InteractionDetection();
        }

        #endregion


        #region 内部方法
        //判断是否可以进行交互
        private bool IsAbleToInteraction(Transform target)
        {
            //没有障碍物
            if (Physics.Raycast(transform.root.position + 0.5f * Vector3.up, target.root.position - transform.root.position,
                out var raycastHit, interactionDetectionRadius, obstacleLayerMask) == false)
            {
                //在NPC前方视线内
                if (Vector3.Dot((target.position - transform.root.position).normalized, transform.root.forward) > viewFieldCosValue)
                {
                    //距离小于限制
                    if(Vector3.Distance(transform.root.position, target.root.transform.position) < interactionDetectionRadius)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检测交互对象
        /// </summary>
        /// <returns></returns>
        private void InteractionDetection()
        {
            //检测到角色
            int targetCount = Physics.OverlapSphereNonAlloc(interactionDetectionCenter.position, interactionDetectionRadius, interactiveTargetColliders, interativeLayerMask);
            if (targetCount > 0)
            {
                //是否能交互
                if (IsAbleToInteraction(interactiveTargetColliders[0].transform.root.transform) == true)
                {
                    //如果能 要么开启对话提示要么开启对话框
                    //设置对话对象
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
                    //不能交互，UI全关
                    SetCurrentInteractionTarget(null);
                    basicUIRefModel.uiManagementRef.OnDisableInteractionInfoPanel();
                    basicUIRefModel.uiManagementRef.OnDisableDialoguePanel();
                }
            }

        }

        /// <summary>
        /// 设置当前交互对象
        /// </summary>
        /// <param name="target">目标对象</param>
        private void SetCurrentInteractionTarget(Transform target) 
        {
            if(currentInteractionTarget == null || currentInteractionTarget != target)
            {
                currentInteractionTarget = target;
            }
        }


        #endregion

        #region 外部方法

        #endregion
    }
}

