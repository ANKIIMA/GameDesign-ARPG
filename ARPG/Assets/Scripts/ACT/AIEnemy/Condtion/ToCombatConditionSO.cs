using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "ToCombatConditionSO", menuName = "StateMachine/Condition/New ToCombatConditionSO")]
public class ToCombatConditionSO : ConditionSO
{
    private AICombat m_aiCombat;
    public override void Init(StateMachineSystem stateSystem)
    {
        m_aiCombat = stateSystem.GetComponent<AICombat>();
    }

    public override bool ConditionSetUp()
    {
        return (m_aiCombat.GetCurrentTarget() != null ? true : false);
    }
}
