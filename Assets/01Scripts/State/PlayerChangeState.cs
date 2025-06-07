using UnityEngine;

public class PlayerChangeState : EntityState
{
    private PlayerChanger _playerChanger;
    private EntityMover _mover;
    private Player _player;
    public PlayerChangeState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _playerChanger = entity.GetCompo<PlayerChanger>();
        _mover = entity.GetCompo<EntityMover>();
        _player = entity as Player;
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
