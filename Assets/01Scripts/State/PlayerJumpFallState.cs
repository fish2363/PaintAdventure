using UnityEngine;

public class PlayerJumpFallState : PlayerAirState
{
    private float _flyTime;

    public PlayerJumpFallState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _flyTime = 0f;
    }
    public override void Update()
    {
        base.Update();

        _flyTime += Time.deltaTime;
        if (_flyTime > 2f)
            _player.ChangeState("FALL");

        if (_mover.IsGroundDetected())
        {
            SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
            skillUIEvent.isHide = false;
            _player.UIChannel.RaiseEvent(skillUIEvent);

            _player.ChangeState("IDLE");
        }
    }
}
