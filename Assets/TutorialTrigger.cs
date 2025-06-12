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
        startTipDialogueEvent.tipText = "��.. ���� �����\n�� �ɽ��ϳ׿� ������ ��\n���� �Ŷ� �׷��� �־����";
        startTipDialogueEvent.tipTrigger = "Draw";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }


    public void DrawABridge()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "�����ؼ� ���� ��\n���̵��� �����ؼ� �ȵſ�\n�ٸ��� �׷��� �־����";
        startTipDialogueEvent.tipTrigger = "";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void DrawWeight()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "��ư�� ��������\n���ſ� �� �ϳ���\n�� �׷����";
        startTipDialogueEvent.tipTrigger = "IronPlate";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void PushMonster()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "����� ģ���� ����\n�����ϴ� ����\n���� �������?";
        startTipDialogueEvent.tipTrigger = "Push";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }
}
