using UnityEngine;

public class CarryObject : WeightObject
{
    private Vector3 _velocity;
    private float _moveSpeedMultiplier=1f;
    private float _pushSpeed = 1f;

    public HandlePoint _handlePoint { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _handlePoint = GetComponentInChildren<HandlePoint>();
    }
}
