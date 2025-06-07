using UnityEngine;

public enum VagetableType
{ 
    Spicy,
    Carrot,
    Apple
}

[RequireComponent(typeof(Rigidbody))]
public class VagetableObject : MonoBehaviour
{
    public VagetableType thisVagetableType;

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
