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
        _mover.SetPushMovement(movementKey);
        if (movementKey.magnitude > _inputThreshold)
            _player.ChangeState("PUSH");
    }
    private void HandleCancelKeyPress()
    {
        _player.GetCompo<ObjectUIComponent>().GetObjUI("PressW/S(Push)").GetComponent<TextMeshPro>().DOFade(0f, 0.2f);

        Object.Destroy(_player.GetComponent<FixedJoint>());
        _player.catchObj._RbCompo.mass = 1000f;

        SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
        skillUIEvent.isHide = false;
        _player.UIChannel.RaiseEvent(skillUIEvent);

        _player.ChangeState("IDLE");
    }
    public override void Exit()
    {
        _player.InputReader.OnJumpKeyEvent -= HandleCancelKeyPress;
        base.Exit();
    }
}
