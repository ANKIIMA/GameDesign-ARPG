using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConreoller : MonoBehaviour
{
    //Necessary Component
    private Transform trans;                                                        //Basic Transfrom Methods
    private Animator animator;                                                          //Interact with the Animation Controller
    private CharacterController characterController;                                    //Dipose the move and collider of the character

    //Intermediate variables
    private float horizontalSpeed;                                                      //Speed in Horizontal Direction
    private float verticalSpeed;                                                        //Speed in Vertical Direction

    // Start is called before the first frame update
    void Start()
    {
        //Get necessary components
        trans = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
