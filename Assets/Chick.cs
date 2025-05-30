using UnityEngine;
using DG.Tweening;

public class Chick : Entity
{
    [SerializeField]
    private ParticleSystem particle;

    private CharacterMovement _movement;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _movement = GetCompo<CharacterMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DOTween.To(() => _movement.gravity = 0f, x => _movement.gravity = x, 0.01f, 0.5f).SetEase(Ease.InOutElastic)
            .OnComplete(() =>
            {
                particle.Play();
                _animator.SetInteger("animation", 8);
            })
            .OnComplete(() => DOTween.To(() => _movement.gravity = 0.5f, x => _movement.gravity = x, -1f, 1f))
            .SetEase(Ease.Unset);
            //.OnComplete(()=> _animator.SetInteger("animation", 0));
        
    }
}
