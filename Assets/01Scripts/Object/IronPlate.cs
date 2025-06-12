using System.Collections;
using UnityEngine;

public class IronPlate : CarryObject
{
    [SerializeField] ParticleSystem destroyEffect;
    private bool isOneTime;
    private void OnCollisionEnter(Collision collision)
    {
        if (isOneTime) return;
        _RbCompo.linearVelocity = Vector3.zero;
        destroyEffect.Play();
        isOneTime = true;
    }

    private void Start()
    {
        StartSafeCoroutine("WaitRoutine", WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(1f);
        _RbCompo.useGravity = true;
        _RbCompo.linearVelocity = Vector3.down * 5;
    }

    private void OnDestroy()
    {
        ParticleSystem particle = Instantiate(destroyEffect);
        particle.Play();
    }
}
