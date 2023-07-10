using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private InputController m_InputController;

    private Vector2 movement;
    private Vector2 cameraLook;
    private bool lAtk;
    private bool rAtk;
    private bool run;
    private bool roll;
    private bool crouch;
    private bool weaponSwitch;
    private bool interact;
    private bool inventory;

    public Vector2 Movement { get => m_InputController.Player.Movement.ReadValue<Vector2>(); }
    public Vector2 CameraLook { get => m_InputController.Player.CameraLook.ReadValue<Vector2>(); }
    public bool LAtk { get => m_InputController.Player.LAtk.triggered; }
    public bool RAtk { get => m_InputController.Player.RAtk.triggered; }
    public bool Run { get => m_InputController.Player.Run.phase == InputActionPhase.Performed; }
    public bool Roll { get => m_InputController.Player.Roll.triggered; }
    public bool Crouch { get => m_InputController.Player.Crouch.triggered; }
    public bool WeaponSwitch { get => m_InputController.Player.WeaponSwitch.triggered; }
    public bool Interact { get => m_InputController.Player.Interact.triggered; }
    public bool Inventory { get => m_InputController.Player.Invetory.triggered; }


    private void Awake()
    {
        if (m_InputController == null)
            m_InputController = new InputController();
    }

    private void OnEnable()
    {
        m_InputController.Enable();
    }

    private void OnDisable()
    {
        m_InputController.Disable();
    }

    #region old way

    /*public void getMovementInput(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void getCameraLookInput(InputAction.CallbackContext context)
    {
        cameraLook = context.ReadValue<Vector2>();
    }

    public void getLAtkInput(InputAction.CallbackContext context)
    {
        lAtk = context.ReadValueAsButton();
    }

    public void getRAtkInput(InputAction.CallbackContext context)
    {
        rAtk = context.ReadValueAsButton();
    }

    public void getRunInput(InputAction.CallbackContext context)
    {
        run = context.ReadValueAsButton();
    }

    public void getRollInput(InputAction.CallbackContext context)
    {
        roll = context.ReadValueAsButton();
    }

    public void getCrouchInput(InputAction.CallbackContext context)
    {
        crouch = context.ReadValueAsButton();
    }*/


    #endregion 


}
