public abstract class EntityState
{
    protected Entity _entity;
    protected int _animationHash;
    protected EntityAnimator _entityAnimator;
    protected EntityAnimatorTrigger[] _animatorTrigger;
    protected bool _isTriggerCall;
    private PlayerChanger _playerChanger;
    public int idx;

    public EntityState(Entity entity, int animationHash)
    {
        _entity = entity;
        _animationHash = animationHash;
        _entityAnimator = entity.GetCompo<EntityAnimator>();
        _animatorTrigger = entity.GetComponentsInChildren<EntityAnimatorTrigger>();
        _playerChanger = entity.GetCompo<PlayerChanger>();
    }

    public virtual void Enter()
    {
        _entityAnimator.SetParam(_animationHash, true);
        _isTriggerCall = false;
        idx = _playerChanger.currentPlayer.playerName == "Bear" ? 0 : 1;
        _animatorTrigger[idx].OnAnimationEndTrigger += AnimationEndTrigger;
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        _entityAnimator.SetParam(_animationHash, false);
        _animatorTrigger[idx].OnAnimationEndTrigger -= AnimationEndTrigger;
    }

    public virtual void AnimationEndTrigger() => _isTriggerCall = true;
}