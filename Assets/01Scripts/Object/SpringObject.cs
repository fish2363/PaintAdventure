    using UnityEngine;

public class SpringObject : PinObject
{
    private Animator _animator;
    [Header("����")]
    [SerializeField]
    private Vector2 jumpForce;
    public bool isOneTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�̾�");
        if (!isOneTime && collision.gameObject.CompareTag("Player"))
        {
            isOneTime = true;
            _animator.SetTrigger("Spring");
            collision.gameObject.GetComponentInChildren<EntityMover>().AddForceToEntity(jumpForce);
        }
    }
    private void OnCanJump()
    {
        Debug.Log(isOneTime);
        isOneTime = false;
    }
}
