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

        SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
        skillUIEvent.isHide = true;
        _player.UIChannel.RaiseEvent(skillUIEvent);

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
        if (velocity.y < -2f)
            _player.ChangeState("FALL");
    }
}
