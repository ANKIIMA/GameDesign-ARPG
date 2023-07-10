using System;
using ACT.Combat;
using ACT.Move;
using UnityEngine;

namespace ACT.Health
{

    public abstract class BasicHealthModel : MonoBehaviour, TakeDamagerInterface
    {

        //ref
        protected Animator animator;
        protected BasicMovementModel movementModel;
        protected BasicCombatModel combatModel;
        protected AudioSource _audioSource;
        [SerializeField] protected UIManagement uiManagement;
        
        //攻击者
        protected Transform currentAttacker;
        
        //AnimationID
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        
        //HitAnimationMoveSpeedMult
        public float hitAnimationMoveMult;

        //属性值
        [SerializeField] protected float healthValue;
        [SerializeField] protected float fullHealthValue;
        [SerializeField] protected float enduranceValue;
        [SerializeField] protected float fullEnduranceValue;
        [SerializeField] protected float normalAttackPower;
        [SerializeField] protected float normalAttackCost;


        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            movementModel = GetComponent<BasicMovementModel>();
            combatModel = GetComponentInChildren<BasicCombatModel>();
            _audioSource = movementModel.GetComponentInChildren<AudioSource>();
        }


        protected virtual void Update()
        {
            HitAnimaitonMove();
        }
        
        
        /// <summary>
        /// 设置攻击者
        /// </summary>
        /// <param name="attacker">攻击者</param>
        public virtual void SetAttacker(Transform attacker)
        {
            if (currentAttacker != attacker || currentAttacker == null)
                currentAttacker = attacker;
        }

        public virtual float CalHealthValuePercentage()
        {
            return healthValue / fullHealthValue;
        }

        public virtual float GetAttackDamage()
        {
            return normalAttackPower;
        }

        protected virtual void HitAnimaitonMove()
        {
            if(!animator.CheckAnimationTag("Hit")) return;
            movementModel.MoveInterface(transform.forward,animator.GetFloat(animationMoveID) * hitAnimationMoveMult,true);
        }

        #region 接口

        public virtual void TakeDamager(float damager)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(string hitAnimationName)
        {
            
        }

        public virtual void TakeDamager(float damager, string hitAnimationName)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(float damager, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);
            animator.Play(hitAnimationName, 0, 0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
            
        }

        public virtual void UpdateDieAnimation()
        {
            animator.Play("Die", 0, 0f);
        }

        #endregion
        
        
        
        
    }
}

