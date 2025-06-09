using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public UnityEvent OnPushEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CanPush"))
        {
            OnPushEvent?.Invoke();
        }
    }
}
