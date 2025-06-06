using UnityEngine;

public class PlayerPushIdleState : PlayerGroundState
{
    private EntityAnimator _animator;
    public PlayerPushIdleState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _animator = _entity.GetCompo<EntityAnimator>();
        _mover.StopImmediately();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("PushIdleÀÓ");
        Vector2 movementKey = _player.InputReader.MovementKey;
        _mover.SetMovementDirection(movementKey);
        if (movementKey.magnitude > _inputThreshold)
        {
            _player.ChangeState("PUSH");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
