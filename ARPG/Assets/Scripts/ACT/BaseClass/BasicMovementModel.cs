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
        protected Vector3 verticalDirection;

        //gravity
        [SerializeField]protected float gravity;
        //internal values
        protected float currentMovementSpeed;
        protected float characterFallTime = 0.15f;
        protected float characterFallOutDeltaTime;
        protected float verticalSpeed;
        protected float maxVerticalSpeed = 53f;

        //Ground Test
        [SerializeField, Header("Ground Test")] protected float detectionRadius;
        [SerializeField] protected float dectectionYOffset;
        [SerializeField] protected float SlopeDetectionRayDistance;
        //Layer Mask of Ground
        [SerializeField] protected LayerMask GroundLayerMask;
        //Layer Mask of Obstacle
        [SerializeField, Tooltip("Obstacle LayerMask")] protected LayerMask ObstacleLayerMask;
        [SerializeField] protected bool isGrounded;

        //AnimationID
        protected int movementID = Animator.StringToHash("Movement");
        protected int horizontalID = Animator.StringToHash("Horizontal");
        protected int verticalID = Animator.StringToHash("Vertical");
        protected int runID = Animator.StringToHash("Run");

        #region Unity事件函数
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            inputController = GetComponent<PlayerInputController>();
            characterController = GetComponent<CharacterController>();
        }

        protected virtual void Start()
        {
            characterFallOutDeltaTime = characterFallTime;
        }

        protected virtual void Update()
        {
            useGravity();
            checkIsGrounded();
        }

        private void OnDrawGizmosSelected()
        {

            //绘制地面检测的范围
            if (isGrounded == true)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            Vector3 position = Vector3.zero;

            position.Set(transform.position.x, transform.position.y - dectectionYOffset,
                transform.position.z);

            Gizmos.DrawWireSphere(position, SlopeDetectionRayDistance);

        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 施加重力
        /// </summary>
        private void useGravity()
        {
            //在地面上
            if(isGrounded)
            {
                characterFallOutDeltaTime = characterFallTime;
                if(verticalSpeed < 0.0f)
                {
                    verticalSpeed = -2f;
                }
            }
            //在空中
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

        /// <summary>
        /// 射线检测是否在地面上
        /// </summary>
        private void checkIsGrounded()
        {
            Vector3 detectPosition = new Vector3(transform.position.x, transform.position.y - dectectionYOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(detectPosition, detectionRadius, GroundLayerMask, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// 修正移动dir
        /// </summary>
        /// <param name="dir">角色移动方向</param>
        /// <returns>修正后的移动方向</returns>
        protected Vector3 SlopeDirAdjustment(Vector3 dir)
        {
            if(Physics.Raycast(transform.position, -Vector3.up, out var hit, SlopeDetectionRayDistance))
            {
                float angle = Vector3.Dot(Vector3.up, hit.normal);
                if(angle != 0 && verticalSpeed <= 0)
                {
                    dir = Vector3.ProjectOnPlane(dir, hit.normal);
                }
            }
            return dir;
        }

        /// <summary>
        /// 检测障碍物
        /// </summary>
        /// <param name="dir">角色移动方向</param>
        /// <returns>是否能继续移动</returns>
        protected bool ObstacleDetection(Vector3 dir)
        {

            return (Physics.Raycast(transform.position + transform.up * .5f, dir.normalized * animator.GetFloat(movementID), out var hit, 1f, ObstacleLayerMask));

        }

        #endregion

        #region 外部方法
        /// <summary>
        /// 移动接口
        /// </summary>
        /// <param name="direction">平面移动方向</param>
        /// <param name="speed">平面移动速度</param>
        /// <param name="gravityOn">重力是否开启</param>
        public virtual void MoveInterface(Vector3 direction, float speed, bool gravityOn)
        {
            if(ObstacleDetection(direction) == false)
            {
                direction = direction.normalized;
                direction = SlopeDirAdjustment(direction);
                direction = speed * direction.normalized;
                if(gravityOn == true)
                {
                    verticalDirection.Set(0.0f, verticalSpeed, 0.0f);
                }
                else
                {
                    verticalDirection = Vector3.zero;
                }

                characterController.Move(direction * Time.deltaTime + verticalDirection * Time.deltaTime);
            }
        }

        #endregion
    }
}

