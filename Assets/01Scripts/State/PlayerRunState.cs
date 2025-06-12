using System;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    private float michineHardCoding;
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

        if (Mathf.Abs(movementKey.magnitude) < _inputThreshold)
        {
            michineHardCoding += Time.deltaTime;
            if (michineHardCoding > 0.1f)
                _player.ChangeState("IDLE");
        }
        else
            michineHardCoding = 0f;
    }

    public override void Exit()
    {
        base.Exit();

        _player.InputReader.OnCancelRunKeyEvent -= CancelRunHandle;
        _mover.SetSprint(false);
    }
}
