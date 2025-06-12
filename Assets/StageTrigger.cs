using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO UIChannel;
    [SerializeField] private ParticleSystem system;
    private int curretObjCount;
    public UnityEvent OnCountFullEvent;
    private bool isEnd;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Tree"))
        {
            Debug.Log("³ª¹«");
            curretObjCount++;
        }
        if (collision.gameObject.CompareTag("Flower"))
        {
            Debug.Log("²É");
            curretObjCount++;
        }
    }

    private void Update()
    {
        if(curretObjCount > 0 && !isEnd)
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
