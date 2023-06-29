using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Move;

namespace ACT.Combat
{
    public abstract class BasicCombatModel : MonoBehaviour
    {
        protected Animator animator;
        protected PlayerInputController inputController;
        //protected BasicMovementModel m_basicMovementModel;
        protected AudioSource audioSource;

        //Attack Detection
        [SerializeField, Header("Attack Detection Center")] protected Transform attackDetectionCenter;
        [SerializeField] protected float attackDetectionRadius;
        [SerializeField] protected LayerMask EnemyLayerMask;


        //aniamtionID
        protected int lAtkID = Animator.StringToHash("LAtk");
        protected int rAtkID = Animator.StringToHash("RAtk");
        //protected int defenID = Animator.StringToHash("Defen");
        //protected int animationMoveID = Animator.StringToHash("AnimationMove");



        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            inputController = GetComponent<PlayerInputController>();
            audioSource = GetComponent<AudioSource>();
        }


        protected virtual void OnAnimationAttackEvent(string hitName)
        {
            if (animator.CheckAnimationTag("Attack") == false) return;

            Collider[] attackDetectionObjects = new Collider[3];

            int counts = Physics.OverlapSphereNonAlloc(attackDetectionCenter.position, attackDetectionRadius, attackDetectionObjects, EnemyLayerMask);

            if(counts > 0)
            {
                for(int i = 0; i < counts; i++)
                {
                    if (attackDetectionObjects[i].TryGetComponent(out IDamagar damagar))
                    {
                        damagar.TakeDamager(hitName);
                    }
                }
            }

            
        }
    }



}

