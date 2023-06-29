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



    #region Unity�¼�����
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

    #region �ڲ�����
    /// <summary>
    /// �Ƿ����ƶ�
    /// </summary>
    /// <returns></returns>
    private bool isAbletoMove()
    {
        return (isGrounded == true && animator.CheckAnimationTag("NormalMotion") || animator.CheckAnimationTag("CrouchMotion"));

    }

    /// <summary>
    /// �Ƿ����¶�
    /// </summary>
    /// <returns></returns>
    private bool isAbletoCrouch()
    {
        if (animator.CheckAnimationTag("CrouchTransition")) return false;
        if (animator.GetFloat(runID) > 0.9f) return false;
        return true;
    }

    /// <summary>
    /// �Ƿ��ܱ���
    /// </summary>
    /// <returns></returns>
    private bool isAbletoRun()
    {
        //ת������Ҫ�ȴ���תͳһ
        if (Vector3.Dot(movementDirection.normalized, transform.forward) < 0.75f) return false;
        if (isAbletoMove() == false) return false;

        return true;
    }

    /// <summary>
    /// ��ɫ��λ�ƺ���ת
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
    /// ���ƽ�ɫ�¶�
    /// </summary>
    private void playerCrouch()
    {
        if (isAbletoCrouch() == false)
        {
            return;
        }
        //�����¶׼�
        if (inputController.Crouch)
        {
            //��ǰ���¶�״̬
            if(isCrouched)
            {
                //ͷ��û���ϰ���
                if(OverHeadObjDetection() == false)
                {
                    //վ��
                    isCrouched = false;
                    animator.SetFloat(crouchID, 0f);
                    SetCrouchColliderValue(originHeight, originCenter);
                    cameraController.setLookAtPosition(normalLook);
                }
                
            }
            //��ǰ�����¶�״̬
            else
            {
                //����
                isCrouched = true;
                animator.SetFloat(crouchID, 1f);
                SetCrouchColliderValue(crouchHeight, crouchCenter);
                cameraController.setLookAtPosition(crouchLook);

            }
        }


    }

    /// <summary>
    /// �����ƶ��Ķ���״̬
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
    /// �����¶׵Ķ���״̬
    /// </summary>
    private void UpdateCrouchAnimation()
    {
        if(isCrouched)
        {
            currentMovementSpeed = crouchMoveSpeed;
        }
    }

    /// <summary>
    /// ���·����Ķ���״̬
    /// </summary>
    private void UpdateRollAnimation()
    {

    }

    /// <summary>
    /// ����ɫͷ���Ƿ����ϰ���
    /// </summary>
    /// <returns>���ؼ����</returns>
    private bool OverHeadObjDetection()
    {
        Collider[] ObjBuffer = new Collider[1];

        int objCount = Physics.OverlapSphereNonAlloc(crouchOverheadDetectionPosition.position, 0.5f, ObjBuffer, crouchDetectionLayerMask);

        return objCount > 0;
    }

    /// <summary>
    /// ����character controller����ײ��߶Ⱥ�����
    /// </summary>
    /// <param name="height">��ײ��߶�</param>
    /// <param name="center">��ײ������</param>
    private void SetCrouchColliderValue(float height, Vector3 center)
    {
        characterController.center = center;
        characterController.height = height;

    }
    
    #endregion


    #region �ⲿ����








    #endregion

}
