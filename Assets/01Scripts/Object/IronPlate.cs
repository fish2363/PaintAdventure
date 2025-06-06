using System.Collections;
using UnityEngine;

public class IronPlate : WeightObject
{
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
}
