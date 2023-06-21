using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//delegate for player properties
public delegate void OnValueChanged(float val);

public class playerModel
{
    //Singleton
    private static playerModel model;

    //player properties
    private float playerHealthPoint = 100f;                                             //health
    private float playerMaxHealthPoint = 100f;
    private float playerManaPoint = 100f;                                               //Skill Point
    private float playerMaxManaPoint = 100f;
    private float manaPointRecovery = 30f;                                              //revovery manapoint value of player
    private float damageValue = 10f;                                                    //damage value of player
    private float attackCost = 50f;                                                     //attack will take 20 manapoint
    private float critcalRate = 1.0f;                                                     //critical rate of attack


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
    private float comboLimitTime = 0.5f;                                                //Limit of the PlayerInput to satisfy a combo
    private int comboIndex = 0;                                                         //index of attack

    //Jump
    private float jumpSpeed = 0f;                                                       //speed of jumping
    private float jumpTargetSpeed = 8f;                                                 //target speed of jumping
    private float gravity = 20f;                                                        //stimulate the gravity

    //Animation Info
    AnimatorStateInfo currentState;                                                     //current state info of the animator

    //delegates for properties
    public OnValueChanged OnPlayerHealthPointChanged;
    public OnValueChanged OnPlayerManaPointChanged;

    public static playerModel GetInstance()
    {
        if (model == null)
            model = new playerModel();

        return model;
    }

    //Constructor
    private playerModel() { }

    #region get and set for properties
    public float PlayerHealthPoint
    {
        get { return playerHealthPoint; }
        set
        {
            playerHealthPoint = value;
            if(OnPlayerHealthPointChanged != null)
            {
                OnPlayerHealthPointChanged(playerHealthPoint);
            }
        }
    }

    public float PlayerManaPoint
    {
        get { return playerManaPoint; }
        set
        {
            playerManaPoint = value;
            if (OnPlayerManaPointChanged != null)
            {
                OnPlayerManaPointChanged(playerManaPoint);
            }
        }
    }

    public float PlayerMaxManaPoint { get => playerMaxManaPoint; set => playerMaxManaPoint = value; }
    public float ManaPointRecovery { get => manaPointRecovery; set => manaPointRecovery = value; }
    public float DamageValue { get => damageValue; set => damageValue = value; }
    public float AttackCost { get => attackCost; set => attackCost = value; }
    public float CritcalRate { get => critcalRate; set => critcalRate = value; }
    public float TargetSpeed { get => targetSpeed; set => targetSpeed = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public Vector3 PlayerTargetDirection { get => playerTargetDirection; set => playerTargetDirection = value; }
    public Vector3 PlayerCurrentDirection { get => playerCurrentDirection; set => playerCurrentDirection = value; }
    public float RotationFactor { get => rotationFactor; set => rotationFactor = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float SpeedFactor { get => speedFactor; set => speedFactor = value; }
    public List<string> ComboNameList { get => comboNameList; set => comboNameList = value; }
    public bool AttackDone { get => attackDone; set => attackDone = value; }
    public float LastAttackTimeInput { get => lastAttackTimeInput; set => lastAttackTimeInput = value; }
    public float ComboCoolDownTime { get => comboCoolDownTime; set => comboCoolDownTime = value; }
    public float ComboLimitTime { get => comboLimitTime; set => comboLimitTime = value; }
    public int ComboIndex { get => comboIndex; set => comboIndex = value; }
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    public float JumpTargetSpeed { get => jumpTargetSpeed; set => jumpTargetSpeed = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public AnimatorStateInfo CurrentState { get => currentState; set => currentState = value; }
    public float PlayerMaxHealthPoint { get => playerMaxHealthPoint; set => playerMaxHealthPoint = value; }

    #endregion


}
