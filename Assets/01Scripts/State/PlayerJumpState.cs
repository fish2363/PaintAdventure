using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _mover.Jump();
        _mover.OnVelocity.AddListener(HandleVelocityChange);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        _mover.OnVelocity.RemoveListener(HandleVelocityChange);
        base.Exit();
    }

    private void HandleVelocityChange(Vector2 velocity)
    {
        if (velocity.y < 0)
            _player.ChangeState("FALL");
    }
}
