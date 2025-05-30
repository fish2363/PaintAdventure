using System;
using UnityEngine;

public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
{
    public Action OnAnimationEndTrigger;
    public event Action OnAttackVFXTrigger;

    private Entity _entity;
    public void Initialize(Entity entity)
    {
        _entity = entity;
    }

    private void AnimationEnd()
    {
        Debug.Log("End");
        OnAnimationEndTrigger?.Invoke();
    }
    
    private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
}
