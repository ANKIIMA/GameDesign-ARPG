using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Health;


    public class AIHealthSystem : BasicHealthModel
    {
        private void LateUpdate()
        {
            OnHitAnimationRotation(); 
        }

        /// <summary>
        /// 接受伤害
        /// </summary>
        /// <param name="damager">伤害值</param>
        /// <param name="hitAnimationName">受伤动画</param>
        /// <param name="attacker">攻击者</param>
        public override void TakeDamager(float damager, string hitAnimationName, Transform attacker)
        {
        base.TakeDamager(damager, hitAnimationName, attacker);
        if(attacker.TryGetComponent<yueduHealthController>(out yueduHealthController healthController))
        {
            damager = healthController.GetAttackDamage();
        }
        healthValue -= damager;
        uiManagement.OnEnemyHealthValueChange(this.CalHealthValuePercentage());
        }

        private void OnHitAnimationRotation()
        {
            if(animator.CheckAnimationTag("Hit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }
}