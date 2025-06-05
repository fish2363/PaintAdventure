using UnityEngine;

public class PlayerInPushState : PlayerGroundState
{
    public PlayerInPushState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _mover.CanManualMove = false;
    }
    public override void Update()
    {
        base.Update();
        if (_isTriggerCall)
            PushHandle();
    }
    private void PushHandle()
    {
        Debug.Log("³ª µÊ¤»");
        _player.ChangeState("PUSH");
    }

    public override void Exit()
    {
        _mover.CanManualMove = true;
        base.Exit();
    }
}
