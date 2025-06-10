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

    private void Awake()
    {
        uiChannel.AddListener<StartTipDialogueEvent>(TipDialogueStartHandle);
        uiChannel.AddListener<StageNameEvent>(StageNameHandle);
        uiChannel.AddListener<SkillUIEvent>(SkillUIChangeHandle);
        uiChannel.AddListener<TutorialEvent>(TutorialEventHandle);
        InputReader.OnTipKeyEvent += TipUI;
    }

    private void TutorialEventHandle(TutorialEvent obj)
    {
        tutorialText.DOFade(1f, 0.2f).OnComplete(() =>
        {
            Debug.Log("와 ㅈ됏논");
            tutorialText.DOFade(1f, 0.2f);
            FindAnyObjectByType<Player>().GetCompo<EntityMover>().CanManualMove = false;
            skipKey = obj.skipKey;
            tutorialText.text =obj.tutorialText;
            isWaitTargetKeyPress = true;

            SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
            skillUIEvent.isHide = true;
            SkillUIChangeHandle(skillUIEvent);
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
            SkillUIChangeHandle(skillUIEvent);
        }
    }
    private void TipUI()
    {
        if (isOnUI)
            KakaoTipOut();
        else
            KakaoTipIn();
        isOnUI = !isOnUI;
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

        InputReader.OnTipKeyEvent -= TipUI;
    }

    private void SkillUIChangeHandle(SkillUIEvent obj)
    {
        if (obj.isHide) UIHide();
        else UIShow();

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
            Debug.Log($"{tutorialEvent.tutorialText}");
            TutorialEventHandle(tutorialEvent);
            tipTextTrigger = "";
        }
        tipNewText.gameObject.SetActive(false);
        kakaoTalk.transform.DOMove(kakaoTalkInPos.position, 1f).SetEase(Ease.OutBack);
    }
    public void UIHide() => chatButtonGroup.alpha = 0f;
    public void UIShow() => DOTween.To(() => chatButtonGroup.alpha, x => chatButtonGroup.alpha = x, 1f, 0.2f);

    public void KakaoTipOut()
    {
        kakaoTalk.transform.DOMove(kakaoTalkOutPos.position, 1f).SetEase(Ease.InExpo);
    }

    private void TipDialogueStartHandle(StartTipDialogueEvent obj)
    {
        tipNewText.gameObject.SetActive(true);
        tipText.text = obj.tipText;
        tipTextTrigger = obj.tipTrigger;
    }
}
