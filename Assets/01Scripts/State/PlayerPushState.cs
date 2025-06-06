using UnityEngine;

public class PlayerPushState : PlayerGroundState
{
    private EntityAnimator _animator;

    public PlayerPushState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _animator = _entity.GetCompo<EntityAnimator>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("PushÀÓ");
        Vector2 movementKey = _player.InputReader.MovementKey;

        if (Mathf.Abs(movementKey.x) > 0 || Mathf.Abs(movementKey.y) > 0 && _mover.CanManualMove)
            _mover.SetPushMovement(movementKey);

        if (movementKey.magnitude < _inputThreshold)
            _player.ChangeState("PUSHIDLE");
    }

}
