using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Move;

public abstract class BasicAbilityModel : ScriptableObject
{
    [SerializeField] private string abilityName;
    [SerializeField] private int abilityIndex;
    [SerializeField] private float abilityCD;
    [SerializeField] private float abilityDistance;
    [SerializeField] private bool abilityReady;

    private Animator animator;
    private AICombat combat;
    private BasicMovementModel movement;


    /// <summary>
    /// 调用技能
    /// </summary>
    public abstract void InvokeAbility();
    /// <summary>
    /// 使用技能
    /// </summary>
    private void UseAbility()
    {
        animator.Play(abilityName, 0, 0);
        abilityReady = false;
        ResetAbility();
    }

    public void ResetAbility()
    {
        GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(abilityCD, () => abilityReady = true, false);
    }



    #region Interface

    public void InitAbility(Animator _animator, AICombat _combat, BasicMovementModel _movement)
    {
        this.animator = _animator;
        this.combat = _combat;
        this.movement = _movement;

    }

    public string GetAbilityName() => abilityName;
    public int GetAbilityIndex() => abilityIndex;
    public bool GetAbilityReady() => abilityReady;
    public void SetAbilityReady(bool ready) => abilityReady = ready;


    #endregion
}
