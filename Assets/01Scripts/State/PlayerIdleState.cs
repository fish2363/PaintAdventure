using UnityEngine;

public class PlayerIdleState : PlayerCanAttackState
{
    private CharacterMovement _movement;

    public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _movement = entity.GetCompo<CharacterMovement>();

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("나 아이들");
    }
    public override void Update()
    {
        base.Update();
        Vector2 movementKey = _player.InputReader.MovementKey;
        _movement.SetMovementDirection(movementKey);
        if (movementKey.magnitude > _inputThreshold)
        {
            _player.ChangeState("MOVE");
        }
    }
}
