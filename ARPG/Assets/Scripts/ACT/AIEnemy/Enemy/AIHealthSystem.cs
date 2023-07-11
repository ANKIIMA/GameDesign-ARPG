using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Health;


    public class AIHealthSystem : BasicHealthModel
    {
    [SerializeField] private GameObject GreatSword;

    private void Start()
    {
        uiManagement.EnemyHealthBar.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

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

        //生命值归零死亡
        if(healthValue <= 0)
        {
            UpdateDieAnimation();
        }
        }

        private void OnHitAnimationRotation()
        {
            if(animator.CheckAnimationTag("Hit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }

    /// <summary>
    /// 死亡动画回调函数
    /// </summary>
    private void OnDieEvent()
    {
        Instantiate(GreatSword, this.transform.position, Quaternion.identity);
        uiManagement.EnemyHealthBar.gameObject.transform.parent.parent.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}