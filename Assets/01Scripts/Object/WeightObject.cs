using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeightObject : ObjectUnit, IDamageable
{
    protected bool isFall;
    public Rigidbody _RbCompo { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        _RbCompo = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(TryGetComponent(out IDamageable damage))
        {

        }
    }
}
