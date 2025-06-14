using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public GameEventChannelSO UIChannel;
    public bool isPlayerPush;

    public UnityEvent OnPushEvent;
    public UnityEvent OnPushExitEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isPlayerPush)
        {
            OnPushEvent?.Invoke();
        }
        if (other.CompareTag("CanPush"))
        {
            OnPushEvent?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CanPush"))
        {
            OnPushExitEvent?.Invoke();
        }
    }

   public void Clear()
    {
        QuestEvent questEvnet = UIEvents.QuestEvent;
        questEvnet.isClear = true;
        questEvnet.duration = 3f;
        UIChannel.RaiseEvent(questEvnet);
    }
}
