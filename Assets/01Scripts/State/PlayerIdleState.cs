using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _mover.StopImmediately();
    }
    public override void Update()
    {
        base.Update();
        Vector2 movementKey = _player.InputReader.MovementKey;
        _mover.SetMovementDirection(movementKey);
        if (movementKey.magnitude > _inputThreshold && _mover.CanManualMove) 
        {
            _player.ChangeState("MOVE");
        }
    }
}
