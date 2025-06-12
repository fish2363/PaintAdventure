using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PushBox : CarryObject
{
    [SerializeField] private Transform spanwPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dead")
        {
            transform.position = spanwPoint.position;
        }
    }
}
