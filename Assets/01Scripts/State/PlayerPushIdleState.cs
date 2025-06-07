using UnityEngine;

public class PlayerPushIdleState : EntityState
{
    protected Player _player;
    protected readonly float _inputThreshold = 0.1f;
    protected EntityMover _mover;
    public PlayerPushIdleState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
        Debug.Assert(_player != null, $"Player state using only in player");
    }
    public override void Enter()
    {
        base.Enter();
        _player.InputReader.OnJumpKeyEvent += HandleCancelKeyPress;
        _mover.StopImmediately();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("PushIdleÀÓ");
        Vector2 movementKey = _player.InputReader.MovementKey;
        _mover.SetPushMovement(movementKey);
        if (movementKey.magnitude > _inputThreshold)
            _player.ChangeState("PUSH");
    }
    private void HandleCancelKeyPress()
    {
        Object.Destroy(_player.GetComponent<FixedJoint>());
        _player.ChangeState("IDLE");
    }
    public override void Exit()
    {
        _player.InputReader.OnJumpKeyEvent -= HandleCancelKeyPress;
        base.Exit();
    }
}
