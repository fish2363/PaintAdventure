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
        startTipDialogueEvent.tipText = "땅이 너무 허전해요\n나무랑 꽃 몇개라도\n그려서 넣어줘요";
        startTipDialogueEvent.tipTrigger = "Draw";
        UIChannel.RaiseEvent(startTipDialogueEvent);

        QuestEvent questEvnet = UIEvents.QuestEvent;
        questEvnet.text = "나무 3개,꽃 1개를\n그려 넣어주세요";
        questEvnet.duration = 3f;
    }

    public void PushMonster()
    {
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "노란색 친구도 같이\n모험하는 편이 좋지 않을까요?";
        startTipDialogueEvent.tipTrigger = "Push";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }
}
