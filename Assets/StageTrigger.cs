using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private int maxflowerCount;
    [SerializeField] private int maxtreeCount;
    [SerializeField] private ParticleSystem system;
    private int curretFlowerCount;
    private int curretTreeCount;
    public UnityEvent OnCountFullEvent;
    private bool isEnd;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Tree"))
        {
            Debug.Log("����");
            curretTreeCount++;
        }
        if (collision.gameObject.CompareTag("Flower"))
        {
            Debug.Log("��");
            curretFlowerCount++;
        }
    }

    private void Update()
    {
        if((curretFlowerCount >= maxflowerCount && curretTreeCount >= maxtreeCount)&&!isEnd)
        {
            isEnd = true;
            system.Play();
            OnCountFullEvent?.Invoke();
            Destroy(this);
        }    
    }
}
