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
        startTipDialogueEvent.tipText = "음.. 지금 여기는\n좀 심심하네요 나무나 꽃\n같은 거라도 그려서 넣어줘요";
        startTipDialogueEvent.tipTrigger = "Draw";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }


    public void DrawABridge()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "수영해서 가는 건\n아이들이 따라해서 안돼요\n다리를 그려서 넣어줘요";
        startTipDialogueEvent.tipTrigger = "";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void DrawWeight()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "버튼을 누를만한\n무거운 거 하나만\n더 그려줘요";
        startTipDialogueEvent.tipTrigger = "IronPlate";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void PushMonster()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "노란색 친구도 같이\n모험하는 편이\n좋지 않을까요?";
        startTipDialogueEvent.tipTrigger = "Push";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }
}
