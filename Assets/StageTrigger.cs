using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO UIChannel;
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
            Debug.Log("³ª¹«");
            curretTreeCount++;
        }
        if (collision.gameObject.CompareTag("Flower"))
        {
            Debug.Log("²É");
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
            QuestEvent questEvnet = UIEvents.QuestEvent;
            questEvnet.isClear = true;
            questEvnet.duration = 3f;
            UIChannel.RaiseEvent(questEvnet);
            Destroy(this);
        }    
    }
}
