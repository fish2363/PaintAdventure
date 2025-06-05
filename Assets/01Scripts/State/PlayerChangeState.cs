using UnityEngine;

public class PlayerChangeState : PlayerGroundState
{
    private PlayerChanger _playerChanger;
    public PlayerChangeState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _playerChanger = entity.GetCompo<PlayerChanger>();
    }

    public override void Enter()
    {
        base.Enter();
        _mover.CanManualMove = false;
        _animatorTrigger[idx].OnAnimationEndTrigger += ChangeHandle;
        _mover.ChangePlayer(_playerChanger.currentPlayer);
    }
    public override void Exit()
    {
        _mover.CanManualMove = true;
        _animatorTrigger[idx].OnAnimationEndTrigger -= ChangeHandle;
        base.Exit();
    }
    private void ChangeHandle()
    {
        _player.ChangeState("IDLE");
    }
}
