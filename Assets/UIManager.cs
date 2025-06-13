using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameEventChannelSO uiChannel;
    [SerializeField]
    private InputReader InputReader;

    [Header("팁UI")]
    [field : SerializeField] private GameObject kakaoTalk;
    [field : SerializeField] private Transform kakaoTalkInPos;
    [field : SerializeField] private Transform kakaoTalkOutPos;
    [field : SerializeField] private CanvasGroup chatButtonGroup;
    [field : SerializeField] private TextMeshProUGUI tipText;
    [field : SerializeField] private Image tipNewText;
    private string tipTextTrigger;

    [Header("스킬UI")]
    public GameObject runSkill;
    public GameObject pushPullSkill;

    [Header("스테이지 이름 UI")]
    [SerializeField]
    private TextMeshProUGUI _stageText;
    [SerializeField]
    private Transform _uiDownPos;
    [SerializeField]
    private Transform _uiUpPos;

    private bool isOnUI;
    [Header("튜토리얼 UI")]
    private bool isWaitTargetKeyPress;
    private KeyCode skipKey;
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    [Header("퀘스트")]
    [SerializeField]
    private TextMeshProUGUI questText;
    [SerializeField]
    private GameObject questBox;
    [SerializeField]
    private Transform questBoxPos;
    private Vector3 prevTrans;

    [Header("UI")]
    [SerializeField] private GesturePaperManager paperManager;

    private bool isProgress;

    private void Awake()
    {
        uiChannel.AddListener<StartTipDialogueEvent>(TipDialogueStartHandle);
        uiChannel.AddListener<StageNameEvent>(StageNameHandle);
        uiChannel.AddListener<QuestEvent>(QuestHandle);
        uiChannel.AddListener<SkillUIEvent>(SkillUIChangeHandle);
        uiChannel.AddListener<TutorialEvent>(TutorialEventHandle);
        uiChannel.AddListener<GestureShow>(GestureShowEventHandle);
        InputReader.OnTipKeyEvent += TipUI;
    }

    private void QuestHandle(QuestEvent obj)
    {
        Debug.Log("아잇잇앗");
        if(!obj.isClear)
        {
            prevTrans = questBox.transform.position;
            questText.text = obj.text;
            questBox.transform.DOMove(questBoxPos.position, obj.duration);
        }
        else
        {
            questText.text = "<color=green>훌륭합니다!</color>";
            questBox.transform.DOMove(prevTrans, obj.duration);
        }
    }

    private void TutorialEventHandle(TutorialEvent obj)
    {
        tutorialText.DOFade(1f, 0.2f).OnComplete(() =>
        {
            tutorialText.DOFade(1f, 0.2f);
            FindAnyObjectByType<Player>().GetCompo<EntityMover>().CanManualMove = false;
            skipKey = obj.skipKey;
            tutorialText.text =obj.tutorialText;
            isWaitTargetKeyPress = true;
            SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
            skillUIEvent.isHide = true;
            uiChannel.RaiseEvent(skillUIEvent);
        }
        );
    }


    private void Update()
    {
        if(isWaitTargetKeyPress && Input.GetKeyDown(skipKey))
        {
            tutorialText.DOFade(0f,0.2f).OnComplete(()=> tutorialText.text ="");
            FindAnyObjectByType<Player>().GetCompo<EntityMover>().CanManualMove = true;
            isWaitTargetKeyPress = false;
            skipKey = KeyCode.None;
            SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
            skillUIEvent.isHide = false;
            uiChannel.RaiseEvent(skillUIEvent);
        }
    }
    private void TipUI()
    {
        if (isProgress) return;
        if (isOnUI)
            KakaoTipOut();
        else
            KakaoTipIn();
        isOnUI = !isOnUI;
        isProgress = true;
    }

    public void KakaoUIFade()
    {
        isOnUI = false;
        KakaoTipOut();
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<StartTipDialogueEvent>(TipDialogueStartHandle);
        uiChannel.RemoveListener<StageNameEvent>(StageNameHandle);
        uiChannel.RemoveListener<SkillUIEvent>(SkillUIChangeHandle);
        uiChannel.RemoveListener<TutorialEvent>(TutorialEventHandle);
        uiChannel.RemoveListener<GestureShow>(GestureShowEventHandle);
        uiChannel.RemoveListener<QuestEvent>(QuestHandle);

        InputReader.OnTipKeyEvent -= TipUI;
    }

    private void GestureShowEventHandle(GestureShow obj)
    {
        if (obj.gestureName == null)
        {
            paperManager.HideUI();
            return;
        }
        paperManager.SetGesturePicture(obj.gestureName);
    }

    private void SkillUIChangeHandle(SkillUIEvent obj)
    {
        if (obj.isHide) UIHide();
        else if(!obj.isHide) UIShow();

        if(obj.type.playerName == "Bear")
        {
            runSkill.SetActive(true);
            pushPullSkill.SetActive(false);
        }
        else
        {
            runSkill.SetActive(false);
            pushPullSkill.SetActive(true);
        }
    }

    private void StageNameHandle(StageNameEvent obj)
    {
        _stageText.text = obj.Text;
        _stageText.rectTransform.DOMove(_uiDownPos.position,obj.duration).SetEase(Ease.OutBounce).OnComplete(()=> _stageText.rectTransform.DOMove(_uiUpPos.position, obj.duration));
    }

    public void KakaoTipIn()
    {
        if(tipTextTrigger == "Draw")
        {
            TutorialEvent tutorialEvent = UIEvents.TutorialEvent;
            tutorialEvent.skipKey = KeyCode.C;
            tutorialEvent.tutorialText = "<swing><color=red>C키</color></swing>를 눌러 <bounce>그림</bounce> 덧그리기";
            Player.IsCanDraw = true;
            TutorialEventHandle(tutorialEvent);
            tipTextTrigger = "";

            QuestEvent questEvnet = UIEvents.QuestEvent;
            questEvnet.text = "나무 또는 꽃을 1개를\n그려 넣어주세요";
            questEvnet.isClear = false;
            questEvnet.duration = 3f;
            uiChannel.RaiseEvent(questEvnet);
        }
        if (tipTextTrigger == "Push")
        {
            TutorialEvent tutorialEvent = UIEvents.TutorialEvent;
            tutorialEvent.skipKey = KeyCode.Tab;
            tutorialEvent.tutorialText = "<swing><color=red>Tab키</color></swing>를 눌러 <bounce>교체</bounce>하기";
            Player.IsCanChange = true;
            TutorialEventHandle(tutorialEvent);
            tipTextTrigger = "";
        }
        if (tipTextTrigger == "IronPlate")
        {
            QuestEvent questEvnet = UIEvents.QuestEvent;
            questEvnet.text = "<swing><color=red>IronPlate</color></swing>를 소환해서\n버튼으로<bounce>밀어보세요</bounce>";
            questEvnet.isClear = false;
            questEvnet.duration = 3f;
            uiChannel.RaiseEvent(questEvnet);

            tipTextTrigger = "";
        }
        FindAnyObjectByType<Player>().ChangeState("IDLE");
        tipNewText.gameObject.SetActive(false);
        kakaoTalk.transform.DOMove(kakaoTalkInPos.position, 1f).SetEase(Ease.OutBack).OnComplete(() => isProgress = false);
    }
    public void UIHide() => chatButtonGroup.alpha = 0f;
    public void UIShow() => chatButtonGroup.alpha = 1f;

    public void KakaoTipOut()
    {
        kakaoTalk.transform.DOMove(kakaoTalkOutPos.position, 0.2f).SetEase(Ease.InExpo).OnComplete(() => isProgress = false);
    }

    private void TipDialogueStartHandle(StartTipDialogueEvent obj)
    {
        KakaoUIFade();
        DOVirtual.DelayedCall(0.3f,()=>
        {
            tipNewText.gameObject.SetActive(true);
            tipText.text = obj.tipText;
            tipTextTrigger = obj.tipTrigger;
        });
    }
}
