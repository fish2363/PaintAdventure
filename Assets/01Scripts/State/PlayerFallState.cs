using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }
    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected())
        {
            _player.ChangeState("IDLE");
        }
    }
}
