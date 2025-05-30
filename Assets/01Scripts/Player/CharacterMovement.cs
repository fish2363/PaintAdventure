using UnityEngine;

public class CharacterMovement : MonoBehaviour, IEntityComponent
{
    [field: SerializeField] public float moveSpeed { get; set; } = 8f;
    [field: SerializeField] public float gravity { get; set; } = -9.81f;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float jumpPower = 5f;


    public bool CanManualMovement { get; set; } = true;
    private Vector3 _autoMovement;
    public bool isGround => characterController.isGrounded;

    private Vector3 _velocity;

    public Vector3 Velocity => _velocity;

    private float _verticalVelocity;
    private Vector3 _movementDirection;

    private Entity _entity;

    public void Initialize(Entity entity)
    {
        _entity = entity;
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
        _movementDirection = forward * movementInput.y + right * movementInput.x;
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        ApplyGravity();
        Move();
    }

    private void CalculateMovement()
    {
        if (CanManualMovement)
        {
            _velocity = _movementDirection;
            _velocity = new Vector3(_velocity.x * moveSpeed, _velocity.y,_velocity.z * moveSpeed) * Time.fixedDeltaTime;
            _velocity *= moveSpeed * Time.fixedDeltaTime;
        }

        if (_velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_velocity);
            Transform parent = _entity.transform;
            parent.transform.rotation = Quaternion.Lerp(parent.transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);

        }
    }

    private void ApplyGravity()
    {
        if (isGround && _verticalVelocity < 0)
            _verticalVelocity = -0.03f;
        else
            _verticalVelocity += gravity * Time.fixedDeltaTime;
        _velocity.y = _verticalVelocity;
    }

    private void Move()
    {
        characterController.Move(_velocity);
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
            _verticalVelocity = jumpPower;
    }

    public void StopImmediately()
    {
        _movementDirection = Vector3.zero;
    }

    public void SetAutoMovement(Vector3 autoMovement)
     => _autoMovement = autoMovement;
}
