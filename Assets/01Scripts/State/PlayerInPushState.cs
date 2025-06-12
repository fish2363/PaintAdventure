using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerInPushState : EntityState
{
    private bool isCompletelyMove;
    protected Player _player;
    protected readonly float _inputThreshold = 0.1f;
    protected EntityMover _mover;

    public PlayerInPushState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
    }

    public override void Enter()
    {
        base.Enter();
        _player.OnPlayerChange?.Invoke(true);
        _player.GetCompo<ObjectUIComponent>().GetObjUI("PressW/S(Push)").GetComponent<TextMeshPro>().DOFade(1f,0.2f);

        _mover.CanManualMove = false;

        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Transform hit in _player.catchObj._handlePoint.handlePoints)
        {
            float distance = Vector3.Distance(_player.transform.position, hit.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = hit.transform;
            }
        }

        _player.transform.DOMove(nearest.position, .5f).OnComplete(() =>
         {
             isCompletelyMove = true;
             Vector3 lookDirection = (_player.catchObj.transform.position - _player.transform.position).normalized;
             _player.transform.rotation = Quaternion.LookRotation(lookDirection);

             FixedJoint joint = _player.gameObject.AddComponent<FixedJoint>();
             joint.connectedBody = _player.catchObj._RbCompo;
             _player.catchObj._RbCompo.mass = 1f;
         });
    }
    public override void Update()
    {
        base.Update();
        if (_isTriggerCall && isCompletelyMove)
            PushHandle();
    }
    private void PushHandle()
    {
        Debug.Log("³ª µÊ¤»");
        _player.ChangeState("PUSHIDLE");
    }

    public override void Exit()
    {
        _mover.CanManualMove = true;
        base.Exit();
    }
}
