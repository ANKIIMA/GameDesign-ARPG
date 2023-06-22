using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    //model instance
    private playerModel model;
    private PlayerController controller;


    //Necessary Component
    private Transform trans;                                                            //Basic Transfrom Methods
    private Animator animator;                                                          //Interact with the Animation Controller
    private CharacterController characterController;                                    //Dipose the move and collider of the character

    

    //Animation Info
    AnimatorStateInfo currentState;                                                     //current state info of the animator

    //Player Input
    Vector2 moveInput;
    bool isRunning;
    bool attack;
    bool jump;

    #region PlayerInput

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void GetRunningInput(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void GetAttackInput(InputAction.CallbackContext context)
    {
        attack = context.ReadValueAsButton();
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        jump = context.ReadValueAsButton();
    }

    #endregion

    public float GetDamageValue()
    {
        return model.DamageValue;
    }

    private void Awake()
    {
        controller = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get model instance
        model = playerModel.GetInstance();

        //Get necessary components
        trans = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();


        //Set the cursor to be invisible in the game
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        
        properitesManagement();
        Locomotion();
        AnimatorInitalize();
        PlayerAttack();
    }

    #region properties Management
    private void properitesManagement()
    {
        HealthPointChanged();
        ManaPointChanged();

    }

    private void HealthPointChanged()
    {

    }

    private void ManaPointChanged()
    {
        model.PlayerManaPoint = Mathf.Clamp(model.PlayerManaPoint + Time.deltaTime * model.ManaPointRecovery, 0f, model.PlayerMaxManaPoint);
    }

    #endregion

    #region AnimatorInitalize
    private void AnimatorInitalize()
    {
        int layerIndex = animator.GetLayerIndex("Base");
        currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);

        //Set the model.AttackDone to false when state is Move
        if (currentState.IsName("Move"))
        {
            model.AttackDone = false;
        }


        //When there is over one second that the player did't input model.Attack
        //transfer the state to move
        if (Time.time - model.LastAttackTimeInput >= model.ComboLimitTime)
        {
            if (currentState.normalizedTime >= 1.0f && currentState.IsTag("Attack"))
            {
                model.ComboIndex = 0;
                model.AttackDone = true;
            }
        }

        animator.SetBool("AttackDone", model.AttackDone);
        animator.SetInteger("ComboIndex", model.ComboIndex);
        animator.SetFloat("Speed", model.CurrentSpeed, 0.1f, Time.deltaTime);
    }

    #endregion

    #region Locomotion
    private void Locomotion()
    {
        //forbid locomotion when attacking
        if (currentState.IsTag("Attack")) return;
        calculateTargetDirection();
        PlayerRotate();
        PlayerJump();
        PlayerMoving();
    }

    //Calculate current and target player direction
    private void calculateTargetDirection()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Camera.main.transform.right.normalized;
        model.PlayerTargetDirection = moveInput.y * cameraForward + moveInput.x * cameraRight;
    }

    //Lerp between model.CurrentSpeed and taragetSpeed
    private void PlayerMoving()
    {
        model.TargetSpeed = isRunning ? model.RunSpeed : model.WalkSpeed;
        model.TargetSpeed = model.TargetSpeed * moveInput.magnitude;

        model.CurrentSpeed = Mathf.Lerp(model.CurrentSpeed, model.TargetSpeed, 0.8f);
        //animator.SetFloat("Speed", model.CurrentSpeed, 0.1f, Time.deltaTime);

        model.PlayerCurrentDirection = Vector3.up * model.JumpSpeed + model.PlayerCurrentDirection * model.CurrentSpeed * model.SpeedFactor;
        characterController.Move(model.PlayerCurrentDirection * Time.deltaTime);
    }

    //Lerp between the current and target Quaternion, then rotate the player
    private void PlayerRotate()
    {
        //Zero Drection will cause no Rotation
        if (model.PlayerTargetDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(model.PlayerTargetDirection);
        Quaternion currentRotation = Quaternion.Slerp(trans.rotation, targetRotation, model.RotationFactor * Time.deltaTime);

        model.PlayerCurrentDirection = trans.forward;

        trans.rotation = currentRotation;
    }

    private void PlayerJump()
    {
        if (characterController.isGrounded)
        {
            if (jump)
            {
                model.JumpSpeed = model.JumpTargetSpeed;
            }
            else
            {
                model.JumpSpeed = 0f;
            }
        }
        else
        {
            model.JumpSpeed -= model.Gravity * Time.deltaTime;
        }


    }
    #endregion

    #region Attack
    private void PlayerAttack()
    {

        //set right model.ComboIndex for playerInput model.Attack
        if (attack && model.PlayerManaPoint >= model.AttackCost)
        {
            //only when state is move, at the same time model.ComboIndex is 0, the model.Attack is permitted
            if (model.ComboIndex == 0)
            {
                model.ComboIndex = 1;
                model.PlayerManaPoint = Mathf.Clamp(model.PlayerManaPoint - model.AttackCost, 0f, model.PlayerMaxManaPoint);
            }
            //transfer to combo2
            else if (model.ComboIndex == 1 && currentState.IsName(model.ComboNameList[model.ComboIndex]) && currentState.normalizedTime >= model.ComboCoolDownTime)
            {
                model.ComboIndex = 2;
                model.PlayerManaPoint = Mathf.Clamp(model.PlayerManaPoint - model.AttackCost, 0f, model.PlayerMaxManaPoint);
            }
            //transfer to combo3
            else if (model.ComboIndex == 2 && currentState.IsName(model.ComboNameList[model.ComboIndex]) && currentState.normalizedTime >= model.ComboCoolDownTime)
            {
                model.ComboIndex = 3;
                model.PlayerManaPoint = Mathf.Clamp(model.PlayerManaPoint - model.AttackCost, 0f, model.PlayerMaxManaPoint);
            }

            else if (model.ComboIndex == 3 && currentState.IsName(model.ComboNameList[model.ComboIndex]) && currentState.normalizedTime >= model.ComboCoolDownTime)
            {
                model.ComboIndex = 1;
                model.PlayerManaPoint = Mathf.Clamp(model.PlayerManaPoint - model.AttackCost, 0f, model.PlayerMaxManaPoint);
            }
            //set the last time that the player input model.Attack, and cost to model.Attack
            model.LastAttackTimeInput = Time.time;
        }

    }

    #endregion

}
