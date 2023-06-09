using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Move;

namespace ACT.Combat
{
    public abstract class BasicCombatModel : MonoBehaviour
    {
        protected Animator animator;
        protected PlayerInputController InputController;
        protected BasicMovementModel MovementBase;
        protected AudioSource audioSource;


        //aniamtionID
        protected int lAtkID = Animator.StringToHash("LAtk");
        protected int rAtkID = Animator.StringToHash("RAtk");
        protected int defenID = Animator.StringToHash("Defen");
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        protected int sWeaponID = Animator.StringToHash("SWeapon");
        protected int lockOnID = Animator.StringToHash("LockOn");

        //攻击检测
        [SerializeField, Header("Attack Detection")] protected Transform attackDetectionCenter;
        [SerializeField] protected float attackDetectionRadius;
        [SerializeField] protected LayerMask EnemyLayerMask;

        #region Unity事件函数
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            InputController = GetComponent<PlayerInputController>();
            MovementBase = GetComponent<BasicMovementModel>();
            audioSource = MovementBase.gameObject.GetComponentInChildren<AudioSource>();
        }

        #endregion


        #region 内部方法
        /// <summary>
        /// 攻击回调函数
        /// </summary>
        /// <param name="hitAnimationName">传敌人受击动画名称</param>
        protected virtual void OnAnimationAttackEvent(string hitAnimationName)
        {
            if (animator.CheckAnimationTag("Attack") == false && animator.CheckAnimationTag("GSAttack") == false) return;

            Collider[] attackDetectionObj = new Collider[4];

            int counts = Physics.OverlapSphereNonAlloc(attackDetectionCenter.position, attackDetectionRadius,
                attackDetectionObj, EnemyLayerMask);
            
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    if (attackDetectionObj[i].TryGetComponent(out TakeDamagerInterface damager))
                    {

                        damager.TakeDamager(0f, hitAnimationName, transform.root.transform);

                    }
                }
            }
            PlayAttackSound();
        }

        /// <summary>
        /// 管理攻击音效
        /// </summary>
        private void PlayAttackSound()
        {
            if(animator.CheckAnimationTag("Attack"))
            {
                GameAssets.Instance.PlaySoundEffect(audioSource, SoundAssetsType.swordWave);
            }
            
            else if(animator.CheckAnimationTag("GSAttack"))
            {
                GameAssets.Instance.PlaySoundEffect(audioSource, SoundAssetsType.gSwordWave);
            }
        }

        #endregion

        #region 外部方法

        #endregion

    }



}

