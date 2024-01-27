using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemReader : Input.IPlayerActions
{
    public event UnityAction<Vector2> AimEvent = delegate { };
    public event UnityAction<Vector2> MoveEvent = delegate { };

    public event UnityAction InteractEvent = delegate { };
    public event UnityAction InteractCancelledEvent = delegate { };
    
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
    
}