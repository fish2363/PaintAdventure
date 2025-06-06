using UnityEngine;

public class CarryObject : ObjectUnit
{
    public HandlePoint _handlePoint { get;private set; }
    protected Rigidbody _RbCompo;

    private void Awake()
    {
        _RbCompo = GetComponent<Rigidbody>();
        _handlePoint = GetComponentInChildren<HandlePoint>();
    }
}
