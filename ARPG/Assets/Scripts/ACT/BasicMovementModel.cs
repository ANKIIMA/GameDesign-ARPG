using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACT.Move
{
    public abstract class BasicMovementModel : MonoBehaviour
    {
        //necessary component
        protected Animator animator;
        protected PlayerInputController inputController;
        protected CharacterController characterController;

        //movement direction
        protected Vector3 movementDirection;
        //gravity
        [SerializeField]protected float gravity;
        //internal values
        protected float currentMovementSpeed;
        protected float characterFallTime = 0.15f;
        protected float characterFallOutDeltaTime;
        protected float verticalSpeed;
        protected float maxVerticalSpeed = 53f;

        [SerializeField, Header("地面检测")] protected float groundDetectionRang;
        [SerializeField] protected float groundDetectionOffset;
        [SerializeField] protected float slopRayExtent;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField, Tooltip("角色动画移动时检测障碍物的层级")] protected LayerMask whatIsObs;
        [SerializeField] protected bool isOnGround;

        //AnimationID
        protected int movementID = Animator.StringToHash("Movement");
        protected int horizontalID = Animator.StringToHash("Horizontal");
        protected int verticalID = Animator.StringToHash("Vertical");
        protected int runID = Animator.StringToHash("Run");

        #region 内部函数
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            inputController = GetComponent<PlayerInputController>();
            characterController = GetComponent<CharacterController>();
        }

        void Start()
        {
            characterFallOutDeltaTime = characterFallTime;
        }

        void Update()
        {
            useGravity();
            checkIsGrounded();
        }
        #endregion

        private void useGravity()
        {
            if(isOnGround)
            {
                characterFallOutDeltaTime = characterFallTime;
                if(verticalSpeed < 0.0f)
                {
                    verticalSpeed = -2f;
                }
            }
            else
            {
                if(characterFallOutDeltaTime >= 0.0f)
                {
                    characterFallOutDeltaTime -= Time.deltaTime;
                    characterFallOutDeltaTime = Mathf.Clamp(characterFallOutDeltaTime, 0, characterFallTime);
                }
            }

            if(verticalSpeed < maxVerticalSpeed)
            {
                verticalSpeed += gravity * Time.deltaTime;
            }
        }

        private void checkIsGrounded()
        {

        }
    }
}

