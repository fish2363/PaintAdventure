using UnityEngine;

public class PlayerAirState : EntityState
{
    protected Player _player;
    protected EntityMover _mover;
    protected PlayerAttackCompo _attackCompo;

    public PlayerAirState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
        _attackCompo = entity.GetCompo<PlayerAttackCompo>();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Vector2 movementKey = _player.InputReader.MovementKey;
        if (Mathf.Abs(movementKey.x) > 0 || Mathf.Abs(movementKey.y) > 0)
            _mover.SetMovementDirection(movementKey);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
