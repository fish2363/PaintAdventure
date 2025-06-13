using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

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
        //구조 ㅈ됏다
        string[] talk = { "음.. 여기 땅에는\n아무것도 안 그리셨네요?", "좀 심심해서요 나무나 꽃\n같은 거라도 그려서 넣어줘요" };

        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;

        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "Draw";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void FlyThePig()
    {
        TutorialEvent tutorialEvent = UIEvents.TutorialEvent;
        tutorialEvent.skipKey = KeyCode.C;
        tutorialEvent.tutorialText = "<swing><color=red>풍선</color></swing>을 <bounce>그려서</bounce> 돼지를 날려보세요";
        UIChannel.RaiseEvent(tutorialEvent);

        QuestEvent questEvnet = UIEvents.QuestEvent;
        questEvnet.text = "풍선을 그려\n돼지를 날려보세요";
        questEvnet.isClear = false;
        questEvnet.duration = 3f;
        UIChannel.RaiseEvent(questEvnet);
    }
    public void ClearBalloon()
    {
        QuestEvent questEvnet = UIEvents.QuestEvent;
        questEvnet.isClear = true;
        questEvnet.duration = 3f;
        UIChannel.RaiseEvent(questEvnet);
    }
    public void DrawABridge()
    {
        string[] talk = { "물을 수영해서 가는 건\n아이들이 따라해서 위험해요", "물을 직접 건너가는 거 말고\n다리를 건너가는 거로 바꿔요","다리 하나 그려볼래요?"};
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void DrawWeight()
    {
        string[] talk = { "아, 여기서 큰 친구가\n능력을 발휘하겠군요", "무거운 걸 옮겨서\n버튼을 누르는 건가요?", "좋네요" };
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "IronPlate";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void PushMonster()
    {
        string[] talk = { "큰 친구도 같이\n걸어가는 편이", "더 좋지 않을까요?", "곰돌이만 너무 나와서요" , "큰 친구로도 교체해봐요"};
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "Push";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }
}
