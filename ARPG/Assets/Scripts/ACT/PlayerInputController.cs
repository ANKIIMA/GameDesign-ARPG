using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    private Vector2 movement;
    private Vector2 cameraLook;
    private bool lAtk;
    private bool rAtk;
    private bool run;
    private bool roll;
    private bool crouch;

    public Vector2 Movement { get => movement; set => movement = value; }
    public Vector2 CameraLook { get => cameraLook; set => cameraLook = value; }
    public bool LAtk { get => lAtk; set => lAtk = value; }
    public bool RAtk { get => rAtk; set => rAtk = value; }
    public bool Run { get => run; set => run = value; }
    public bool Roll { get => roll; set => roll = value; }
    public bool Crouch { get => crouch; set => crouch = value; }


    #region

    public void getMovementInput(InputAction.CallbackContext context)
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
    }


    #endregion
}
