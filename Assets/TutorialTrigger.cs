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
        //���� ���Ѵ�
        string[] talk = { "��.. ���� ������\n�ƹ��͵� �� �׸��̳׿�?", "�� �ɽ��ؼ��� ������ ��\n���� �Ŷ� �׷��� �־����" };

        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;

        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "Draw";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void FlyThePig()
    {
        TutorialEvent tutorialEvent = UIEvents.TutorialEvent;
        tutorialEvent.skipKey = KeyCode.C;
        tutorialEvent.tutorialText = "<swing><color=red>ǳ��</color></swing>�� <bounce>�׷���</bounce> ������ ����������";
        UIChannel.RaiseEvent(tutorialEvent);

        QuestEvent questEvnet = UIEvents.QuestEvent;
        questEvnet.text = "ǳ���� �׷�\n������ ����������";
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
        string[] talk = { "���� �����ؼ� ���� ��\n���̵��� �����ؼ� �����ؿ�", "���� ���� �ǳʰ��� �� ����\n�ٸ��� �ǳʰ��� �ŷ� �ٲ��","�ٸ� �ϳ� �׷�������?"};
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void DrawWeight()
    {
        string[] talk = { "��, ���⼭ ū ģ����\n�ɷ��� �����ϰڱ���", "���ſ� �� �Űܼ�\n��ư�� ������ �ǰ���?", "���׿�" };
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "IronPlate";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }

    public void PushMonster()
    {
        string[] talk = { "ū ģ���� ����\n�ɾ�� ����", "�� ���� �������?", "�����̸� �ʹ� ���ͼ���" , "ū ģ���ε� ��ü�غ���"};
        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = talk;
        startTipDialogueEvent.tipTrigger = "Push";
        UIChannel.RaiseEvent(startTipDialogueEvent);
    }
}
