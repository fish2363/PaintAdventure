using System;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "SO/Input",fileName = "newInput")]
public class InputReader : ScriptableObject,PlayerInput.IPlayerActions
{
    private Vector3 _worldPosition;

    [SerializeField] private LayerMask whatIsGround;

    public Vector2 MovementKey { get; private set; }
    private PlayerInput inputs;

    public event Action<bool> OnDrawingEvent;
    private bool isDrawing;

    public event Action OnJumpKeyEvent;
    public event Action OnUniqueActivityKeyEvent;
    public event Action OnCancelRunKeyEvent;
    public event Action OnChangeKeyEvent;
    public event Action OnESCKeyEvent;

    public event Action OnTipKeyEvent;

    private bool isOnDraw=false;

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
        if (context.performed)
        {
            if(!isDrawing)
            {
                OnDrawingEvent?.Invoke(true);
                isDrawing = true;
            }
            else
            {
                OnDrawingEvent?.Invoke(false);
                isDrawing = false;
            }
        }
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
        if (context.performed)
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

    public Vector3 GetWorldPosition(out RaycastHit hit)
    {
        Camera mainCam = Camera.main;
        Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(camRay, out hit, mainCam.farClipPlane, whatIsGround);
        if (isHit)
        {
            _worldPosition = hit.point;
        }
        return _worldPosition;
    }

    public void OnESC(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnESCKeyEvent?.Invoke();
        }
    }
}
