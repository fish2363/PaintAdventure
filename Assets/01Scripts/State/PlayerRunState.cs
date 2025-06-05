using System;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _mover.SetSprint(true);
        _player.InputReader.OnCancelRunKeyEvent += CancelRunHandle;
    }

    private void CancelRunHandle()
    {
        _player.ChangeState("IDLE");
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

    public override void Exit()
    {
        base.Exit();
        _player.InputReader.OnCancelRunKeyEvent -= CancelRunHandle;
        _mover.SetSprint(false);
    }
}
