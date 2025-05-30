using UnityEngine;

public class ObjectUnit : ExtendedMono
{
    protected Rigidbody _RbCompo;

    private void Awake()
    {
        _RbCompo = GetComponent<Rigidbody>();
    }


}
