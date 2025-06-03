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

        if (_mover.CanManualMove)
            _mover.SetMovement(movementKey);

        if (movementKey.magnitude < _inputThreshold || !_mover.CanManualMove)
            _player.ChangeState("IDLE");
    }
}
