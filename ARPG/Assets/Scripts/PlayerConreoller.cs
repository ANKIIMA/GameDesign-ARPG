using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConreoller : MonoBehaviour
{
    //Necessary Component
    private Transform trans;                                                            //Basic Transfrom Methods
    private Animator animator;                                                          //Interact with the Animation Controller
    private CharacterController characterController;                                    //Dipose the move and collider of the character

    //Intermediate variables
    private float targetSpeed;                                                          //Target Speed of character
    private float currentSpeed = 0;                                                     //Current Speed of character
    private Vector3 playerTargetDirection;                                              //Target Direction of the Camera
    private Vector3 playerCurrentDirection;                                             //Current Direction of the player
    private float rotationFactor = 6f;                                                  //Smooth Factor of locomotion
    private float runSpeed = 2f;                                                        //Speed when running
    private float walkSpeed = 1f;                                                       //Speed when walking
    private float speedFactor = 3f;                                                     //Speed Scale Factor


    //Player Input
    Vector2 moveInput;
    bool isRunning;


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

    #endregion
    
    // Update is called once per frame
    void Update()
    {
        calculateTargetDirection();
        PlayerRotate();
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

        characterController.SimpleMove(playerCurrentDirection * currentSpeed * speedFactor);

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

}
