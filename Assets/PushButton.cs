using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public UnityEvent OnPushEvent;
    public UnityEvent OnPushExitEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CanPush"))
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
}
