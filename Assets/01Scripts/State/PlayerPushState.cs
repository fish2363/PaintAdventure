using DG.Tweening;
using TMPro;
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
        Debug.Log("Push¿”");
        Vector2 movementKey = _player.InputReader.MovementKey;

        if (_mover.IsGroundDetected() == false)
        {
            _player.GetCompo<ObjectUIComponent>().GetObjUI("PressW/S(Push)").GetComponent<TextMeshPro>().DOFade(0f, 0.2f);
            Object.Destroy(_player.GetComponent<FixedJoint>());
            _player.catchObj._RbCompo.mass = 1000f;
            _player.catchObj._RbCompo.linearVelocity = Vector3.zero;

            SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
            skillUIEvent.isHide = false;
            _player.UIChannel.RaiseEvent(skillUIEvent);

            _player.ChangeState("IDLE");
        }

        _entityAnimator.animator.SetFloat("Y", movementKey.y);

        if (Mathf.Abs(movementKey.y) > 0 && _mover.CanManualMove)
            _mover.SetPushMovement(movementKey);

        if (movementKey.magnitude < _inputThreshold || !_mover.CanManualMove)
            _player.ChangeState("PUSHIDLE");
    }

}
