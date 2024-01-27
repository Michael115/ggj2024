using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemReader : Input.IPlayerActions
{
    public event UnityAction<Vector2> AimEvent = delegate { };
    public event UnityAction<Vector2> MoveEvent = delegate { };

    public event UnityAction InteractEvent = delegate { };
    public event UnityAction InteractCancelledEvent = delegate { };
    
    public event UnityAction ShootEvent = delegate { };
    public event UnityAction ShootCancelledEvent = delegate { };
    
    public event UnityAction ShootSecondaryEvent = delegate { };
    public event UnityAction ShootSecondaryCancelledEvent = delegate { };
    
    private Input _gameInput;
    
    public InputSystemReader(Input rawInput)
    {
        _gameInput = rawInput;
        _gameInput.Player.Enable();
        _gameInput.Player.SetCallbacks(this);
    }

    public void DisableAllInput()
    {     
        _gameInput.Player.Disable();
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        AimEvent.Invoke(context.ReadValue<Vector2>());
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            InteractCancelledEvent.Invoke();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ShootEvent.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            ShootCancelledEvent.Invoke();
        }
    }

    public void OnShootSecondary(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ShootSecondaryEvent.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            ShootCancelledEvent.Invoke();
        }
    }
}