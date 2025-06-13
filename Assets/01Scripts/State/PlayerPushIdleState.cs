using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerPushIdleState : EntityState
{
    protected Player _player;
    protected readonly float _inputThreshold = 0.1f;
    protected EntityMover _mover;
    private float _flyTime;

    public PlayerPushIdleState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
        Debug.Assert(_player != null, $"Player state using only in player");
    }
    public override void Enter()
    {
        base.Enter();
        _player.InputReader.OnJumpKeyEvent += HandleCancelKeyPress;
        _mover.StopImmediately();
    }

    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected() == false)
            HandleCancelKeyPress();

        Vector2 movementKey = _player.InputReader.MovementKey;
        if (Mathf.Abs(movementKey.y) > _inputThreshold && _mover.CanManualMove)
            _player.ChangeState("PUSH");
    }
    private void HandleCancelKeyPress()
    {
        _player.OnPlayerChange?.Invoke(false);
        _player.GetCompo<ObjectUIComponent>().GetObjUI("PressW/S(Push)").GetComponent<TextMeshPro>().DOFade(0f, 0.2f);

        Object.Destroy(_player.GetComponent<FixedJoint>());
        _player.catchObj._RbCompo.mass = 1000f;
        _player.catchObj._RbCompo.linearVelocity = Vector3.zero;

        _player.ChangeState("IDLE");
    }
    public override void Exit()
    {
        _player.InputReader.OnJumpKeyEvent -= HandleCancelKeyPress;
        base.Exit();
    }
}
