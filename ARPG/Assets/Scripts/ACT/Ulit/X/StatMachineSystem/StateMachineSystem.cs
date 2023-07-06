using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StateMachineSystem : MonoBehaviour
{
    [Required("转换条件容器不得为空!")]
    public NB_Transition transition;

    [Required("默认状态不得为空!")]
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
        transition?.TryGetApplyCondition();//每一帧都去找是否有成立的条件
        currentState?.OnUpdate();


    }
}
