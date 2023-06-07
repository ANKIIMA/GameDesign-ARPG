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
    private float horizontalTargetSpeed;                                                //Target Speed in Horizontal Direction
    private float horizontalCurrentSpeed;                                               //Current Speed in Horizontal Direction
    private float verticalTargetSpeed;                                                  //Speed in Vertical Direction
    private float verticalCurrentSpeed;                                                 //Current Speed in Vertical Direction
    private Vector3 playerDirection;                                                    //Target Direction of the Camera



    [SerializeField, SetProperty("Run Speed")]                                          //Speed when running
    private float runSpeed = 2;
    [SerializeField, SetProperty("Walk Speed")]                                         //Speed when walking
    private float walkSpeed = 1;

    

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
        PlayerMoving();
    }

    private void calculateTargetDirection()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Camera.main.transform.right;
        playerDirection = moveInput.y * cameraForward + moveInput.x * cameraRight;
        Debug.Log(playerDirection);
    }

    //Only deal with the Animation, OnAnimatorMove() include the real movement
    //Lerp between currentSpeed and taragetSpeed
    private void PlayerMoving()
    {
        verticalTargetSpeed = isRunning ? runSpeed : walkSpeed;
        animator.SetFloat("Vertical Speed", verticalTargetSpeed * moveInput.magnitude, 0.1f, Time.deltaTime);
    }
    
    private void PlayerRotate()
    {
        float rad = Mathf.Atan2(playerDirection.x, playerDirection.z);

    }

    private void OnAnimatorMove()
    {
        //characterController.Move(animator.velocity);
    }
}
