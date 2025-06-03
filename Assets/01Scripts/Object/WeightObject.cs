using UnityEngine;

public class WeightObject : CarryObject, IDamageable
{
    protected bool isFall;
    private void OnCollisionEnter(Collision collision)
    {
        if(TryGetComponent(out IDamageable damage))
        {

        }
    }
}
