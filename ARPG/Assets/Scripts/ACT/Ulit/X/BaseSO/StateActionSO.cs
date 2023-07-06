using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateActionSO : ScriptableObject
{
    [SerializeField] protected int statePriority;//状态优先级
    
    public virtual void OnEnter(StateMachineSystem stateMachineSystem) { }

    public abstract void OnUpdate();

    public virtual void OnExit() { }

    /// <summary>
    /// 获取状态优先级
    /// </summary>
    /// <returns></returns>
    public int GetStatePriority() => statePriority;
}
