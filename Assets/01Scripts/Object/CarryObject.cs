using UnityEngine;

public class CarryObject : ObjectUnit
{
    protected Rigidbody _RbCompo;

    private void Awake()
    {
        _RbCompo = GetComponent<Rigidbody>();
    }
}
