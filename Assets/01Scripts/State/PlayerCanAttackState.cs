using UnityEngine;

public class PlayerCanAttackState : PlayerState
{
    public PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
       // _player.InputReader.OnAttackPressed += HandleAttackPressed;
    }

    public override void Exit()
    {
       // _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
        base.Exit();
    }

    private void HandleAttackPressed()
    {
        _player.ChangeState("ATTACK");
    }
}
