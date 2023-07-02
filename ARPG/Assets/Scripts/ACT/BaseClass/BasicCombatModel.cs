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

        //�������
        [SerializeField, Header("Attack Detection")] protected Transform attackDetectionCenter;
        [SerializeField] protected float attackDetectionRadius;
        [SerializeField] protected LayerMask EnemyLayerMask;

        #region Unity�¼�����
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            InputController = GetComponent<PlayerInputController>();
            MovementBase = GetComponent<BasicMovementModel>();
            audioSource = MovementBase.gameObject.GetComponentInChildren<AudioSource>();
        }

        #endregion


        #region �ڲ�����
        /// <summary>
        /// �����ص�����
        /// </summary>
        /// <param name="hitAnimationName">�������ܻ���������</param>
        protected virtual void OnAnimationAttackEvent(string hitAnimationName)
        {
            if (animator.CheckAnimationTag("Attack") == false) return;

            Collider[] attackDetectionObj = new Collider[4];

            int counts = Physics.OverlapSphereNonAlloc(attackDetectionCenter.position, attackDetectionRadius,
                attackDetectionObj, EnemyLayerMask);
            
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    if (attackDetectionObj[i].TryGetComponent(out IDamager damager))
                    {
                        Debug.Log(attackDetectionObj[i].name);
                        Debug.Log(hitAnimationName);
                        damager.TakeDamager(hitAnimationName);

                    }
                }
            }
            GameAssets.Instance.PlaySoundEffect(audioSource, SoundAssetsType.swordWave);
        }

        #endregion

        #region �ⲿ����

        #endregion

    }



}

