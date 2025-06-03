using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Vector2 movementKey = _player.InputReader.MovementKey;

        if (Mathf.Abs(movementKey.x) > 0 || Mathf.Abs(movementKey.y) > 0 && _mover.CanManualMove)
            _mover.SetMovementDirection(movementKey);

        if (movementKey.magnitude < _inputThreshold)
            _player.ChangeState("IDLE");
    }
}
