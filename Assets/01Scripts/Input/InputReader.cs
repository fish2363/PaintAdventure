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
    public event Action OnJumpKeyEvent;
    public event Action OnUniqueActivityKeyEvent;
    public event Action OnCancelRunKeyEvent;
    public event Action OnChangeKeyEvent;

    public event Action OnTipKeyEvent;

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
        if (context.performed)
            OnJumpKeyEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementKey = context.ReadValue<Vector2>();
    }

    public void OnUniqueActivity(InputAction.CallbackContext context)
    {
        if (context.started)
            OnUniqueActivityKeyEvent?.Invoke();
        if (context.canceled)
            OnCancelRunKeyEvent?.Invoke();
    }

    public void OnPlayerChange(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnChangeKeyEvent?.Invoke();
    }

    public void OnTip(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnTipKeyEvent?.Invoke();
    }
}
