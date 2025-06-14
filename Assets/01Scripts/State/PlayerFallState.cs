using Ami.BroAudio;
using System.Collections;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{

    public PlayerFallState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();


        if (_mover.IsGroundDetected())
        {
            SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
            skillUIEvent.isHide = false;
            _player.UIChannel.RaiseEvent(skillUIEvent);
            BroAudio.Play(_player.fallSound);
            _player.ChangeState("ENDFALL");
        }
    }
}
