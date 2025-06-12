using UnityEngine;

public class PlayerEndFallState : PlayerAirState
{
    public PlayerEndFallState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _mover.StopImmediately();
        _animatorTrigger[idx].OnGetUpEvent += ChangeIdle;
        _mover.CanManualMove = false;
    }
    public override void Exit()
    {
        _animatorTrigger[idx].OnGetUpEvent -= ChangeIdle;
        _mover.CanManualMove = true;
        base.Exit();
    }
    private void ChangeIdle()
    {
        _player.ChangeState("IDLE");
    }
}
