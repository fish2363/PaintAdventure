using Ami.BroAudio;
using System;
using UnityEngine;

public class EntityAnimatorTrigger : MonoBehaviour
{
    public SoundID laandingSFX;
    public Action OnAnimationEndTrigger;
    public event Action OnAttackVFXTrigger;
    public event Action OnGetUpEvent;

    private EntityMMFeedback feedback;
    //구조 진짜ㅇ언ㅁ루말ㅇㅁ웆보ㅠㅗ뮹
    private void Start()
    {
        feedback = FindAnyObjectByType<EntityMMFeedback>();
    }

    private void AnimationEnd()
    {
        Debug.Log("End");
        OnAnimationEndTrigger?.Invoke();
    }

    private void PlaySoundLanding() => BroAudio.Play(laandingSFX);
    private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
    private void PlayCameraShakeFeedback() => feedback.PlayFeedback("CameraImpulse");
    private void AfterFallingGetUp() => OnGetUpEvent?.Invoke();
}
