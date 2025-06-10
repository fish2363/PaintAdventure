using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private GameEventChannelSO UIChannel;
    public UnityEvent OnTriggerEvent;
    private bool isOnetime;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isOnetime==false)
        {
            OnTriggerEvent?.Invoke();
            isOnetime = true;   
        }
    }

    public void MoreDrawPls()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "���� �ʹ� �����ؿ�\n���� ���\n�׷��� �־����";
        startTipDialogueEvent.tipTrigger = "Draw";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }
}
