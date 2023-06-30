using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Move;

namespace ACT.Combat
{
    public abstract class BasicCombatModel : MonoBehaviour
    {
        protected Animator _animator;
        protected PlayerInputController InputController;
        protected BasicMovementModel MovementBase;
        protected AudioSource audioSource;


        //aniamtionID
        protected int lAtkID = Animator.StringToHash("LAtk");
        protected int rAtkID = Animator.StringToHash("RAtk");
        protected int defenID = Animator.StringToHash("Defen");
        protected int animationMoveID = Animator.StringToHash("AnimationMove");

        //¹¥»÷¼ì²â
        [SerializeField, Header("¹¥»÷¼ì²â")] protected Transform attackDetectionCenter;
        [SerializeField] protected float attackDetectionRang;
        [SerializeField] protected LayerMask enemyLayer;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            InputController = GetComponent<PlayerInputController>();
            MovementBase = GetComponent<BasicMovementModel>();
            audioSource = MovementBase.gameObject.GetComponentInChildren<AudioSource>();
        }





        /// <summary>
        /// ¹¥»÷¶¯»­¹¥»÷¼ì²âÊÂ¼þ
        /// </summary>
        /// <param name="hitName">´«µÝÊÜÉË¶¯»­Ãû</param>
        protected virtual void OnAnimationAttackEvent(string hitName)
        {
            if (!_animator.CheckAnimationTag("Attack")) return;

            Collider[] attackDetectionTargets = new Collider[4];

            int counts = Physics.OverlapSphereNonAlloc(attackDetectionCenter.position, attackDetectionRang,
                attackDetectionTargets, enemyLayer);

            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    if (attackDetectionTargets[i].TryGetComponent(out IDamager damager))
                    {
                        damager.TakeDamager(hitName);

                    }
                }
            }
            GameAssets.Instance.PlaySoundEffect(audioSource, SoundAssetsType.swordWave);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawWireSphere(attackDetectionCenter.position, attackDetectionRang);
        }
    }



}

