using UnityEngine;
using UnityEngine.Events;

public class StaticAnimalObjects : PinObject
{
    [SerializeField]
    private LayerMask whatIsVagetable;
    [SerializeField] private VagetableType myWantVagetable;
    public UnityEvent OnHappyAnimal;
    public UnityEvent OnAngryAnimal;
    public UnityEvent OnflYAnimal;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask whatIsPlayer;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, whatIsVagetable);
        for(int i=0;i<colliders.Length; i++)
        {
            if(colliders[i].TryGetComponent(out VagetableObject vagetable))
            {
                if (vagetable.thisVagetableType == myWantVagetable)
                {
                    Destroy(colliders[i].gameObject);
                    OnHappyAnimal?.Invoke();
                }
                else
                {
                    Destroy(colliders[i].gameObject);
                    OnAngryAnimal?.Invoke();
                }
            }
        }

        if (Physics.CheckSphere(transform.position, radius, whatIsPlayer))
        {
            Debug.Log("¾ßÈ£");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,radius);
        Gizmos.color = Color.white;
    }
}
