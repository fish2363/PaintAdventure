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
        Debug.Log("�̾�");
        _animator.SetTrigger("Spring");
    }
}
