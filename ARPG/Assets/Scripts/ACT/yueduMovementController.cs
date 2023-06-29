using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Move;
public class yueduMovementController : BasicMovementModel
{
    //camera components
    private Transform camera3rd;
    private CameraController cameraController;

    //Lock on Position
    [SerializeField, Header("Lock on Position")] private Transform normalLook;
    [SerializeField] private Transform crouchLook;

    //Ref Value
    private float targetRotation;
    private float rotationVelocity;

    //LerpTime
    [SerializeField, Header("Rotation Smooth")] private float rotationLerpTime;
    [SerializeField] private float moveDirctionSlerpTime;


    //Move Speed
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchMoveSpeed;


    [SerializeField, Header("Crouch")] private Vector3 crouchCenter;
    [SerializeField] private Vector3 originCenter;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float originHeight;

    [SerializeField] private bool isCrouched;

    [SerializeField] private Transform crouchOverheadDetectionPosition;
    [SerializeField] private LayerMask crouchDetectionLayerMask;

    //animationID
    private int crouchID = Animator.StringToHash("Crouch");



    #region Unity事件函数
    protected override void Awake()
    {
        base.Awake();

        camera3rd = Camera.main.transform.root.transform;
        cameraController = camera3rd.GetComponent<CameraController>();
    }

    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        playerTransform();
    }

    private void LateUpdate()
    {
        playerCrouch();
        UpdateMotionAnimation();
        UpdateCrouchAnimation();
        UpdateRollAnimation();

    }
    #endregion

    #region 内部方法
    /// <summary>
    /// 是否能移动
    /// </summary>
    /// <returns></returns>
    private bool isAbletoMove()
    {
        return (isGrounded == true && animator.CheckAnimationTag("NormalMotion") || animator.CheckAnimationTag("CrouchMotion"));

    }

    /// <summary>
    /// 是否能下蹲
    /// </summary>
    /// <returns></returns>
    private bool isAbletoCrouch()
    {
        if (animator.CheckAnimationTag("CrouchTransition")) return false;
        if (animator.GetFloat(runID) > 0.9f) return false;
        return true;
    }

    /// <summary>
    /// 是否能奔跑
    /// </summary>
    /// <returns></returns>
    private bool isAbletoRun()
    {
        //转身奔跑需要等待旋转统一
        if (Vector3.Dot(movementDirection.normalized, transform.forward) < 0.75f) return false;
        if (isAbletoMove() == false) return false;

        return true;
    }

    /// <summary>
    /// 角色的位移和旋转
    /// </summary>
    private void playerTransform()
    {
        if(isGrounded == true && inputController.Movement == Vector2.zero)
        {
            movementDirection = Vector3.zero;
        }

        if(isAbletoMove())
        {
            if(inputController.Movement != Vector2.zero)
            {
                //rotation
                targetRotation = Mathf.Atan2(inputController.Movement.x, inputController.Movement.y) * Mathf.Rad2Deg + camera3rd.localEulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationLerpTime);
                //position
                Vector3 direction = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
                movementDirection = Vector3.Slerp(movementDirection, SlopeDirAdjustment(direction.normalized), moveDirctionSlerpTime * Time.deltaTime);
            }
        }
        else
        {
            movementDirection = Vector3.zero;
        }

        characterController.Move(currentMovementSpeed * Time.deltaTime * movementDirection.normalized + 
            Time.deltaTime * new Vector3(0.0f, verticalSpeed, 0.0f));
    }

    /// <summary>
    /// 控制角色下蹲
    /// </summary>
    private void playerCrouch()
    {
        if (isAbletoCrouch() == false)
        {
            return;
        }
        //按下下蹲键
        if (inputController.Crouch)
        {
            //当前是下蹲状态
            if(isCrouched)
            {
                //头顶没有障碍物
                if(OverHeadObjDetection() == false)
                {
                    //站立
                    isCrouched = false;
                    animator.SetFloat(crouchID, 0f);
                    SetCrouchColliderValue(originHeight, originCenter);
                    cameraController.setLookAtPosition(normalLook);
                }
                
            }
            //当前不是下蹲状态
            else
            {
                //蹲下
                isCrouched = true;
                animator.SetFloat(crouchID, 1f);
                SetCrouchColliderValue(crouchHeight, crouchCenter);
                cameraController.setLookAtPosition(crouchLook);

            }
        }


    }

    /// <summary>
    /// 更新移动的动画状态
    /// </summary>
    private void UpdateMotionAnimation()
    {
        if(isAbletoRun())
        {
            float movementValue = inputController.Movement.sqrMagnitude * ((inputController.Run && !isCrouched) ? 2f : 1f);
            animator.SetFloat(movementID, movementValue, 0.1f, Time.deltaTime);
            currentMovementSpeed = (inputController.Run && !isCrouched) ? runSpeed : walkSpeed;
        }
        else
        {
            animator.SetFloat(movementID, 0f, 0.05f, Time.deltaTime);
            currentMovementSpeed = 0f;
        }

        animator.SetFloat(runID, (inputController.Run && !isCrouched) ? 1f : 0f);
    }

    /// <summary>
    /// 更新下蹲的动画状态
    /// </summary>
    private void UpdateCrouchAnimation()
    {
        if(isCrouched)
        {
            currentMovementSpeed = crouchMoveSpeed;
        }
    }

    /// <summary>
    /// 更新翻滚的动画状态
    /// </summary>
    private void UpdateRollAnimation()
    {

    }

    /// <summary>
    /// 检测角色头顶是否有障碍物
    /// </summary>
    /// <returns>返回检测结果</returns>
    private bool OverHeadObjDetection()
    {
        Collider[] ObjBuffer = new Collider[1];

        int objCount = Physics.OverlapSphereNonAlloc(crouchOverheadDetectionPosition.position, 0.5f, ObjBuffer, crouchDetectionLayerMask);

        return objCount > 0;
    }

    /// <summary>
    /// 设置character controller的碰撞体高度和中心
    /// </summary>
    /// <param name="height">碰撞体高度</param>
    /// <param name="center">碰撞体中心</param>
    private void SetCrouchColliderValue(float height, Vector3 center)
    {
        characterController.center = center;
        characterController.height = height;

    }
    
    #endregion


    #region 外部方法








    #endregion

}
