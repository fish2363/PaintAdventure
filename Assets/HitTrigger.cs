using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("ÀÌ¾å");
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 collisionNormal = collision.contacts[0].normal;
            Vector3 bounceDirection = collisionNormal.normalized * 50f;
            Debug.Log("Èû Àû¿ë: " + bounceDirection);
            bounceDirection.y = 20f;
            collision.gameObject.GetComponentInChildren<EntityMover>().KnockBack(bounceDirection,0.5f);
        }
    }
}
