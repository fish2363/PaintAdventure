using UnityEngine;

public class PlayerGroundState : EntityState
{
    protected Player _player;
    protected readonly float _inputThreshold = 0.1f;
    protected EntityMover _mover;

    public PlayerGroundState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
        Debug.Assert(_player != null, $"Player state using only in player");
    }
    public override void Enter()
    {
        base.Enter();
        _player.InputReader.OnJumpKeyEvent += HandleJumpKeyPress;
    }

    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected() == false && _mover.CanManualMove)
        {
            _player.ChangeState("FALL");
        }
    }
    public override void Exit()
    {
        _player.InputReader.OnJumpKeyEvent -= HandleJumpKeyPress;
        base.Exit();
    }

    private void HandleJumpKeyPress()
    {
        if (_mover.IsGroundDetected())
            _player.ChangeState("JUMP");
    }
}
