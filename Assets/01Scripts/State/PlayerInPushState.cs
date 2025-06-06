using DG.Tweening;
using UnityEngine;

public class PlayerInPushState : PlayerGroundState
{
    private bool isCompletelyMove;
    public PlayerInPushState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
        
        _player.transform.DOMove(nearest.position,.5f).OnComplete(()=>
        {
            isCompletelyMove = true;

            Vector3 lookDirection = (nearest.position - _player.transform.position).normalized;
            _player.transform.rotation = Quaternion.LookRotation(lookDirection);
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
