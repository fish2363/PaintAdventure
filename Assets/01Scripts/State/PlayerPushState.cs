using UnityEngine;

public class PlayerPushState : EntityState
{
    protected Player _player;
    protected readonly float _inputThreshold = 0.1f;
    protected EntityMover _mover;
    public PlayerPushState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
        Debug.Assert(_player != null, $"Player state using only in player");
    }

    public override void Enter()
    {
        base.Enter();
        _mover.CanRotateBody = false;
    }

    public override void Exit()
    {
        _mover.CanRotateBody = true;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("PushÀÓ");
        Vector2 movementKey = _player.InputReader.MovementKey;

        if (Mathf.Abs(movementKey.x) > 0 || Mathf.Abs(movementKey.y) > 0 && _mover.CanManualMove)
            _mover.SetPushMovement(movementKey);

        if (movementKey.magnitude < _inputThreshold)
            _player.ChangeState("PUSHIDLE");
    }

}
