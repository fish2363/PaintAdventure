using System;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "SO/Input",fileName = "newInput")]
public class InputReader : ScriptableObject,PlayerInput.IPlayerActions
{
    [SerializeField] private LayerMask whatIsGround;

    public Vector2 MovementKey { get; private set; }
    private PlayerInput inputs;

    public event Action<bool> OnDrawingEvent;

    private void OnEnable()
    {
        if (inputs == null)
        {
            inputs = new();
            inputs.Player.SetCallbacks(this);
        }
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        
    }

    public void OnDraw(InputAction.CallbackContext context)
    {
        if (context.started)
            OnDrawingEvent?.Invoke(true);
        else if (context.canceled)
            OnDrawingEvent?.Invoke(false);
    }

    public void OnInteractable(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementKey = context.ReadValue<Vector2>();
    }


}
