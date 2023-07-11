using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Health;

public class yueduHealthController : BasicHealthModel
{
    private void LateUpdate()
    {
        OnHitAnimationRotation();
    }


    #region �ⲿ����

    public void TakePortion(string portionName)
    {
        switch(portionName)
        {
            case "����ҩˮ":
                healthValue = Mathf.Clamp(100, 0f, fullHealthValue);
                break;
        }
        uiManagement.OnPlayerHealthValueChange(this.CalHealthValuePercentage());
    }


    /// <summary>
    /// ��д���ܷ���
    /// </summary>
    /// <param name="damager"></param>
    /// <param name="hitAnimationName"></param>
    /// <param name="attacker"></param>
    public override void TakeDamager(float damager, string hitAnimationName, Transform attacker)
    {
        base.TakeDamager(damager, hitAnimationName, attacker);
        if (attacker.TryGetComponent<AIHealthSystem>(out AIHealthSystem healthController))
        {
            damager = healthController.GetAttackDamage();
        }
        healthValue -= damager;
        uiManagement.OnPlayerHealthValueChange(this.CalHealthValuePercentage());
    }



    #endregion

    #region �ڲ�����
    private void OnHitAnimationRotation()
    {
        if (animator.CheckAnimationTag("Hit"))
        {
            transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
        }
    }



    #endregion
}
