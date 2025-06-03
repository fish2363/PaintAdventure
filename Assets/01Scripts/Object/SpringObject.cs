    using UnityEngine;

public class SpringObject : PinObject
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ÀÌ¾å");
        _animator.SetTrigger("Spring");
    }
}
