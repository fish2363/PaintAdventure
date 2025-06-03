using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Chick : Entity,IEntityComponent
{
    private EntityMover _movement;
    private Animator _animator;
    private EntityVFX _entityVFX;

    public void Initialize(Entity entity)
    {
        _movement = entity.GetCompo<EntityMover>();
        _animator = GetComponent<Animator>();
        _entityVFX = entity.GetCompo<EntityVFX>();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartSafeCoroutine("AppearanceRoutine", AppearanceRoutine());
    }

    private IEnumerator AppearanceRoutine()
    {
        _movement.RbCompo.useGravity = false;
        _entityVFX.PlayVfx("FeatherExplosion", Vector3.zero, Quaternion.identity);
        _animator.SetInteger("animation", 3);
        yield return new WaitForSeconds(1f);
        _animator.SetInteger("animation", 0);
        _movement.RbCompo.useGravity = true;
    }
}
