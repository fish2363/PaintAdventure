using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class EntityMover : MonoBehaviour, IEntityComponent
{
    #region Member field
    private Entity _entity;
    public UnityEvent<Vector2> OnVelocity;
    [field: SerializeField] public float WalkSpeed { get; set; }
    [field: SerializeField] public float RunSpeed { get; set; }
    private float moveSpeed;

    private Vector3 _velocity;
    private float _moveSpeedMultiplier;

    public Rigidbody RbCompo { get; private set; }
    private Vector3 _autoMovement;

    [SerializeField]
    private float _jumpPower;
    [SerializeField] private float rotationSpeed = 8f;

    #endregion

    [Header("Collision detection")]
    [SerializeField] private Transform groundCheckTrm;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public bool CanManualMove { get; set; } = true;


    public void Initialize(Entity entity)
    {
        _entity = entity;
        RbCompo = entity.GetComponent<Rigidbody>();
        Debug.Assert(RbCompo != null, "이거 없음");
        _moveSpeedMultiplier = 1f;
        moveSpeed = WalkSpeed;
    }
    public void SetMovementDirection(Vector2 movementInput)
    {
        var cam = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        _velocity = forward * movementInput.y + right * movementInput.x;
    }
    public void ChangePlayer(PlayerType type)
    {
        WalkSpeed = type.WalkSpeed;
        groundCheckDistance = type.GroundCheckDistance;
    }

    public void SetPushMovement(Vector2 movementInput)
    {
        var forward = _entity.transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude > 0.001f)
            forward.Normalize();

        _velocity = forward * movementInput.y;
    }

    private void FixedUpdate()
    {
        if (CanManualMove)
        {
            Vector3 newVelocity = _velocity * moveSpeed * _moveSpeedMultiplier;
            newVelocity.y = RbCompo.linearVelocity.y;
            RbCompo.linearVelocity = newVelocity;

            if (_velocity.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_velocity);
                Transform parent = _entity.transform;
                parent.transform.rotation = Quaternion.Lerp(parent.transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }
        }
        OnVelocity?.Invoke(RbCompo.linearVelocity);
    }

    public void StopImmediately()
    {
        _velocity = Vector3.zero;
        RbCompo.linearVelocity = Vector3.zero;
    }

    public void SetSprint(bool run) => moveSpeed = run ? RunSpeed : WalkSpeed;

    public void Jump()
    {
        Vector3 jumpVelocity = Vector3.up  * _jumpPower;
        AddForceToEntity(jumpVelocity);
    }

    public void AddForceToEntity(Vector3 force)
        => RbCompo.AddForce(force, ForceMode.Impulse);
    public void SetAutoMovement(Vector3 autoMovement)
         => _autoMovement = autoMovement;
    public void KnockBack(Vector2 force, float time)
    {
        CanManualMove = false;
        StopImmediately();
        AddForceToEntity(force);
        DOVirtual.DelayedCall(time, () => CanManualMove = true);
    }
    #region Check Collision

    public bool IsGroundDetected()
    {
        return Physics.Raycast(groundCheckTrm.position, Vector3.down, groundCheckDistance,whatIsGround);
    }

    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (groundCheckTrm != null)
        {
            Gizmos.DrawRay(groundCheckTrm.position, Vector3.down * groundCheckDistance);
        }
    }
#endif

}