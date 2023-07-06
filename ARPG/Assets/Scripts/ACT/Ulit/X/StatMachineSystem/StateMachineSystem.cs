using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StateMachineSystem : MonoBehaviour
{
    [Required("ת��������������Ϊ��!")]
    public NB_Transition transition;

    [Required("Ĭ��״̬����Ϊ��!")]
    public StateActionSO currentState;


    private void Awake()
    {
        transition?.Init(this);
        currentState?.OnEnter(this);
    }


    private void Update()
    {
        StateMachineTick();
    }

    private void StateMachineTick() 
    {
        transition?.TryGetApplyCondition();//ÿһ֡��ȥ���Ƿ��г���������
        currentState?.OnUpdate();


    }
}
