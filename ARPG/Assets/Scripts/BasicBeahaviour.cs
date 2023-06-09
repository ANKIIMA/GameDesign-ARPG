using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicBeahaviour : MonoBehaviour
{
    //Necessary Component
    private Transform trans;                                                            //Basic Transfrom Methods
    private Animator animator;                                                          //Interact with the Animation Controller
    private CharacterController characterController;                                    //Dipose the move and collider of the character

    //player properties
    private float playerHealth;                                                         //health
    private float playerSkillPoint;                                                     //Skill Point
    private float damageValue = 10f;                                                    //damage value of player
    

    //Intermediate variables
    //Locomotion
    private float targetSpeed;                                                          //Target Speed of character
    private float currentSpeed = 0;                                                     //Current Speed of character
    private Vector3 playerTargetDirection;                                              //Target Direction of the Camera
    private Vector3 playerCurrentDirection;                                             //Current Direction of the player
    private float rotationFactor = 6f;                                                  //Smooth Factor of locomotion
    private float runSpeed = 2f;                                                        //Speed when running
    private float walkSpeed = 1f;                                                       //Speed when walking
    private float speedFactor = 3f;                                                     //Speed Scale Factor

    //Fight
    List<string> comboNameList = new List<string> { "Move", "AttackA", "AttackB", "AttackC" };  //Name of combo Animations
    private bool attackDone;                                                            //judge if a single attack is done playing
    private float lastAttackTimeInput;                                                  //Time of last Attack input
    private float comboCoolDownTime = 0.8f;                                             //the necessary animating time of a single Attack
    private float comboLimitTime = 1.0f;                                                //Limit of the PlayerInput to satisfy a combo
    private int comboIndex = 0;                                                         //index of attack

    //Jump
    private float jumpSpeed = 0f;                                                       //speed of jumping
    private float jumpTargetSpeed = 8f;                                                 //target speed of jumping
    private float gravity = 20f;                                                        //stimulate the gravity

    //Animation Info
    AnimatorStateInfo currentState;                                                     //current state info of the animator


    //Player Input
    Vector2 moveInput;
    bool isRunning;
    bool attack;
    bool jump;

    public float PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public float PlayerSkillPoint { get => playerSkillPoint; set => playerSkillPoint = value; }
    public float DamageValue { get => damageValue; set => damageValue = value; }


    // Start is called before the first frame update
    void Start()
    {
        //Get necessary components
        trans = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        //Set the cursor to be invisible in the game
        Cursor.lockState = CursorLockMode.Locked;

    }

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
    
    // Update is called once per frame
    void Update()
    {
        AnimatorInitalize();
        Locomotion();
        PlayerAttack();
    }


    private void AnimatorInitalize()
    {
        int layerIndex = animator.GetLayerIndex("Base");
        currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);


        //Set the attackDone to false when state is Move
        if (currentState.IsName("Move"))
        {
            attackDone = false;
            animator.SetBool("AttackDone", attackDone);
        }

        if(characterController.isGrounded)
        {

        }

        //When there is over one second that the player did't input attack
        //transfer the state to move
        if (Time.time - lastAttackTimeInput >= comboLimitTime)
        {
            if (currentState.normalizedTime >= 1.0f && currentState.IsTag("Attack"))
            {
                comboIndex = 0;
                animator.SetInteger("ComboIndex", comboIndex);
                attackDone = true;
                animator.SetBool("AttackDone", attackDone);
            }
        }
    }


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
        playerTargetDirection = moveInput.y * cameraForward + moveInput.x * cameraRight;
    }

    //Lerp between currentSpeed and taragetSpeed
    private void PlayerMoving()
    {
        targetSpeed = isRunning ? runSpeed : walkSpeed;
        targetSpeed = targetSpeed * moveInput.magnitude;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 0.8f);
        animator.SetFloat("Speed", currentSpeed, 0.1f, Time.deltaTime);

        playerCurrentDirection = Vector3.up * jumpSpeed + playerCurrentDirection * currentSpeed * speedFactor;
        characterController.Move(playerCurrentDirection * Time.deltaTime);
    }
    
    //Lerp between the current and target Quaternion, then rotate the player
    private void PlayerRotate()
    {
        //Zero Drection will cause no Rotation
        if (playerTargetDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(playerTargetDirection);
        Quaternion currentRotation = Quaternion.Slerp(trans.rotation, targetRotation, rotationFactor * Time.deltaTime);

        playerCurrentDirection = trans.forward;

        trans.rotation = currentRotation;
    }

    private void PlayerJump()
    {
        if(characterController.isGrounded)
        {
            if(jump)
            {
                jumpSpeed = jumpTargetSpeed;
            }
            else
            {
                jumpSpeed = 0f;
            }
        }
        else
        {
            jumpSpeed -= gravity * Time.deltaTime;
        }


    }
    #endregion

    #region Attack
    private void PlayerAttack()
    {
        
        //set right comboIndex for playerInput attack
        if (attack)
        {
            //only when state is move, at the same time comboIndex is 0, the attack is permitted
            if(comboIndex == 0)
            {
                comboIndex = 1;
                animator.SetInteger("ComboIndex", comboIndex);
            }
            //transfer to combo2
            else if(comboIndex == 1 && currentState.IsName(comboNameList[comboIndex]) && currentState.normalizedTime >= comboCoolDownTime){
                comboIndex = 2;
                animator.SetInteger("ComboIndex", comboIndex);
            }
            //transfer to combo3
            else if(comboIndex == 2 && currentState.IsName(comboNameList[comboIndex]) && currentState.normalizedTime >= comboCoolDownTime){
                comboIndex = 3;
                animator.SetInteger("ComboIndex", comboIndex);
                comboIndex = 0;
            }
            //set the last time that the player input attack
            lastAttackTimeInput = Time.time;
        }

    }
    #endregion

}
