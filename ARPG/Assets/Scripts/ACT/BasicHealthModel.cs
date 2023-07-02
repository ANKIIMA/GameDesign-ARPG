using System;
using ACT.Combat;
using ACT.Move;
using UnityEngine;

namespace ACT.Health
{
    public abstract class BasicHealthModel : MonoBehaviour, IDamager
    {
        
        //引用
        protected Animator _animator;
        protected BasicMovementModel movementModel;
        protected BasicCombatModel combatModel;
        protected AudioSource _audioSource;
        
        //攻击者
        protected Transform currentAttacker;
        
        //AnimationID
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        
        //HitAnimationMoveSpeedMult
        public float hitAnimationMoveMult;


        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
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

        protected virtual void HitAnimaitonMove()
        {
            if(!_animator.CheckAnimationTag("Hit")) return;
            movementModel.MoveInterface(transform.forward,_animator.GetFloat(animationMoveID) * hitAnimationMoveMult,true);
        }

        #region 接口

        public virtual void TakeDamager(float damager)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(string hitAnimationName)
        {
            _animator.Play(hitAnimationName,0,0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
        }

        public virtual void TakeDamager(float damager, string hitAnimationName)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);
            
        }

        #endregion
        
        
        
        
    }
}

