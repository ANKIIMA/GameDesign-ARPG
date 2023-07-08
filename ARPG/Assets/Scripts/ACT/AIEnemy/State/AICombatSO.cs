using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AICombatSO", menuName = "StateMachine/State/New AICombatSO")]
public class AICombatSO : StateActionSO
{
    Animator animator;
    AICombat m_aiCombat;
    AIMovement m_aiMovement;

    private int lAtkID = Animator.StringToHash("LAtk");
    private int rAtkID = Animator.StringToHash("RAtk");
    private int defenID = Animator.StringToHash("Defen");
    private int animationMoveID = Animator.StringToHash("AnimationMove");
    private int sWeaponID = Animator.StringToHash("SWeapon");
    private int verticalID = Animator.StringToHash("Vertical");
    private int horizontalID = Animator.StringToHash("Horizontal");

    private int RandomValue;


    #region ��д����
    public override void OnUpdate()
    {
        NoCombat();
    }


    public override void OnEnter(StateMachineSystem stateMachineSystem)
    {
        base.OnEnter(stateMachineSystem);

        animator = stateMachineSystem.GetComponent<Animator>();
        m_aiCombat = stateMachineSystem.GetComponent<AICombat>();
        m_aiMovement = stateMachineSystem.GetComponent<AIMovement>();

        if (animator == null || m_aiCombat == null || m_aiMovement == null)
        {
            Debug.Log("component null");
        }
    }
    #endregion


    #region �ڲ�����
    private void NoCombat()
    {
        if (animator.CheckAnimationTag("NormalMotion")) 
        {
            //��������
            if(m_aiCombat.GetCurrentTargetDistance() < 3f + 0.1f)
            {
                m_aiMovement.MoveInterface(-m_aiMovement.transform.forward, 1.4f, true);
                animator.SetFloat(verticalID, -1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, 0f, 0.25f, Time.deltaTime);

                RandomValue = GetRandomValue();

                if(m_aiCombat.GetCurrentTargetDistance() < 1.7f)
                {
                    RandomValue = GetRandomValue();
                    //animator.Play("Slide_B", 0, 0f);
                    //m_aiMovement.MoveInterface(m_aiMovement.transform.forward, animator.GetFloat(animationMoveID) * 10, true);
                    switch (RandomValue)
                    {
                        case -1:
                            animator.Play("Atk1", 0, 0);
                            break;
                        case 0:
                            animator.Play("Atk2", 0, 0);
                            break;

                        case 1:
                            animator.Play("Atk3", 0, 0);
                            break;
                    }
                    m_aiMovement.MoveInterface(m_aiMovement.transform.forward, animator.GetFloat(animationMoveID) * 10, true);
                }
            }
            
            //�����ʵ�������ƶ�,�����������󷭹�
            else if(m_aiCombat.GetCurrentTargetDistance() >3f && m_aiCombat.GetCurrentTargetDistance() < 7f)
            {
                //�����ƶ�
                if(RandomValue == 1 || RandomValue == -1)
                {
                    m_aiMovement.MoveInterface(m_aiMovement.transform.right * RandomValue, 1.5f, true);
                }

                animator.SetFloat(verticalID, 0f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, RandomValue, 0.25f, Time.deltaTime);
            }

            //Զ��ǰ��
            else if(m_aiCombat.GetCurrentTargetDistance() > 7f && m_aiCombat.GetCurrentTargetDistance() < 10f)
            {
                m_aiMovement.MoveInterface(m_aiMovement.transform.forward, 1.5f, true);
                animator.SetFloat(verticalID, -1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, 0f, 0.25f, Time.deltaTime);
            }
            //̫Զ��׷
            else if(m_aiCombat.GetCurrentTargetDistance() > 10f)
            {
                animator.SetFloat(verticalID, 0f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, 0f, 0.25f, Time.deltaTime);
            }
        }
    }

    private int GetRandomValue() => Random.Range(-1, 2);

    #endregion
}
