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

    private void Awake()
    {
        uiChannel.AddListener<StartTipDialogueEvent>(TipDialogueStartHandle);
        uiChannel.AddListener<StageNameEvent>(StageNameHandle);
        uiChannel.AddListener<SkillUIEvent>(SkillUIChangeHandle);
        InputReader.OnTipKeyEvent += TipUI;
    }
    private void Start()
    {
        StartTipDialogueEvent startTipDialogueEvent = new();
        startTipDialogueEvent.tipText = "히히히히";
        TipDialogueStartHandle(startTipDialogueEvent);
    }
    private void TipUI()
    {
        if (isOnUI)
            KakaoTipOut();
        else
            KakaoTipIn();
        isOnUI = !isOnUI;
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<StartTipDialogueEvent>(TipDialogueStartHandle);
        uiChannel.RemoveListener<StageNameEvent>(StageNameHandle);
        uiChannel.RemoveListener<SkillUIEvent>(SkillUIChangeHandle);
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
        tipNewText.gameObject.SetActive(false);
        kakaoTalk.transform.DOMove(kakaoTalkInPos.position, 1f).SetEase(Ease.OutBack);
        UIHide();
    }
    public void UIHide() => chatButtonGroup.alpha = 0f;
    public void UIShow() => DOTween.To(() => chatButtonGroup.alpha, x => chatButtonGroup.alpha = x, 1f, 0.2f);

    public void KakaoTipOut()
    {
        kakaoTalk.transform.DOMove(kakaoTalkOutPos.position, 1f).SetEase(Ease.InExpo).OnComplete(() => UIShow());
    }

    private void TipDialogueStartHandle(StartTipDialogueEvent obj)
    {
        tipNewText.gameObject.SetActive(true);
        tipText.text = obj.tipText;
    }
}
