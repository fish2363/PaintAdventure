using System;
using UnityEngine;

public class PlayerGroundState : EntityState
{
    protected Player _player;
    protected readonly float _inputThreshold = 0.1f;
    protected EntityMover _mover;

    private float _flyTime;

    private bool isJump;

    public PlayerGroundState(Entity entity, int animationHash) : base(entity, animationHash)
    {
        _player = entity as Player;
        _mover = entity.GetCompo<EntityMover>();
        Debug.Assert(_player != null, $"Player state using only in player");
    }
    public override void Enter()
    {
        base.Enter();
        _flyTime = 0f;
        _player.InputReader.OnChangeKeyEvent += HandleChangePlayer;
        _player.InputReader.OnJumpKeyEvent += HandleJumpKeyPress;
        _player.InputReader.OnUniqueActivityKeyEvent += OnUniqueActivityHandle;
    }

    private void HandleChangePlayer()
    {
        if(Player.IsCanChange)
        _player.ChangePlayer();
    }
    

    private void OnUniqueActivityHandle()
    {
        if (_player.CurrentPlayer().playerName == "Bear" && _mover.CanManualMove)
            _player.ChangeState("RUN");
        else
        {
            Collider[] target = Physics.OverlapSphere(_player.transform.position,_player.detectionDistance,_player.whatIsInteractableObj);
            if (target.Length < 1) return;

            float minDistance = Mathf.Infinity;
            Transform nearest = null;

            foreach (Collider hit in target)
            {
                if (!hit.TryGetComponent(out CarryObject carryObject)) return;
                float distance = Vector3.Distance(_player.transform.position, hit.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = hit.transform;
                }
            }

            _player.catchObj = nearest.GetComponent<CarryObject>();
            _player.ChangeState("INPUSH");
        }
    }

    public override void Update()
    {
        base.Update();
        if (_mover.IsGroundDetected() == false && _mover.CanManualMove && !isJump)
        {
            _flyTime += Time.deltaTime;
            if (_flyTime > 0.5f)
                _player.ChangeState("FALL");
        }
    }
    public override void Exit()
    {
        _player.InputReader.OnChangeKeyEvent -= HandleChangePlayer;
        _player.InputReader.OnJumpKeyEvent -= HandleJumpKeyPress;
        _player.InputReader.OnUniqueActivityKeyEvent -= OnUniqueActivityHandle;
        isJump = false;
        base.Exit();
    }


    private void HandleJumpKeyPress()
    {
        if (_mover.CanManualMove&&_mover.IsGroundDetected() && _player.CurrentPlayer().playerName == "Bear" && _mover.CanJump)
        {
            _player.ChangeState("JUMP");
        }
    }
}
