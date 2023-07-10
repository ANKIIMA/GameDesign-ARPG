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

    #region Unity�¼�����
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


    #region �ڲ�����

    /// <summary>
    /// �ж��Ƿ��ܹ����빥��
    /// </summary>
    /// <returns></returns>
    private bool CanInputAttack()
    {
        //bug: ����0.25�ı�׼��ʱ��Ҳ�ᷢ��״̬ת�ƣ�
        if (animator.CheckCurrentAnimationTagTimeIsGreater("Attack", 0.22f))
        {
            return true;
        }
        if (animator.CheckCurrentAnimationTagTimeIsGreater("GSAttack", 0.30f))
        {
            return true;
        }

        if (animator.CheckAnimationTag("NormalMotion") || animator.CheckAnimationTag("GSMotion")) return true;


        //�ѽ�� Triggerδ���� ����״̬����Trigger�����������һ��
        //ԭ������false�󾭹���ε��Triggerʵ��ֵ��Ȼ��true
        animator.ResetTrigger(lAtkID);
        animator.ResetTrigger(rAtkID);
        return false;
    }


    /// <summary>
    /// ���¹�������
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
    /// ���ƹ���ʱ���ƶ�
    /// </summary>
    private void ActionMotion()
    {
        if (animator.CheckAnimationTag("Attack") || animator.CheckAnimationTag("GSAttack"))
        {
            MovementBase.MoveInterface(transform.forward, animator.GetFloat(animationMoveID) * animationRootAttackScale, true);
        }
    }


    #region �����Զ�����
    /// <summary>
    /// ��ת����������
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
    /// �жϹ���״̬�Ƿ������Զ���������
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
    /// ���ǰ������
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
    /// ���õ�ǰ����Ŀ��
    /// </summary>
    /// <param name="target">��������</param>
    private void SetCurrentTarget(Transform target)
    {
        if(currentTarget == null || currentTarget != target)
        {
            currentTarget = target;
        }
    }

    /// <summary>
    /// ��������״̬
    /// �ƶ�ʱ�����Զ�����
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
