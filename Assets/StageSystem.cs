using DG.Tweening;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class StageSystem : ExtendedMono
{
    public GameObject scetch;
    public Ease ease;
    public PlayableDirector _director;
    public StageSet[] stageSet;
    private int currentStage;
    public Material stageCamera;
    [field:SerializeField] public bool IsClear { get; set; }

    [SerializeField]
    private GameEventChannelSO uiChannel;

    [Header("디죨브")]
    [SerializeField] private SkinnedMeshRenderer[] dissolvesMaterials;

    private void Start()
    {
        StageNameEvent stageNameEvent = UIEvents.StageNameEvent;
        stageNameEvent.Text = stageSet[currentStage].stageName;
        stageNameEvent.duration = 3f;
        uiChannel.RaiseEvent(stageNameEvent);

        if(stageSet[currentStage].stageName =="시작의 초원")
        {
            StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
            startTipDialogueEvent.tipText = "곰돌이가 주인공?\n일단 만화를 더 봐야\n알겠네요 앞으로 가요\n지켜보면서 피드백\n해드릴게요";
            uiChannel.RaiseEvent(startTipDialogueEvent);
            FirstStartTutorial();
        }
        GestureShow gestureShow = UIEvents.GestureShow;
        gestureShow.gestureName = stageSet[currentStage].drawPictureName;
        uiChannel.RaiseEvent(gestureShow);

        FindAnyObjectByType<Player>().ReStartSet(false, stageSet[currentStage].drawPictureName);
    }

    private void FirstStartTutorial()
    {
        SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
        skillUIEvent.isHide = true;
        uiChannel.RaiseEvent(skillUIEvent);

        TutorialEvent tutorialEvent = UIEvents.TutorialEvent;
        tutorialEvent.skipKey = KeyCode.Q;
        tutorialEvent.tutorialText = "<swing><color=red>Q키</color></swing>를 눌러 <bounce>메신저</bounce> 확인하기";
        uiChannel.RaiseEvent(tutorialEvent);
    }

    [ContextMenu("StageClear")]
    public void StageClear()
    {
        IsClear = false;
        FindAnyObjectByType<Player>().SaveToSecondPos(null);
        FindAnyObjectByType<Player>().GetCompo<EntityMover>().RbCompo.isKinematic = true;
        stageSet[currentStage].stage.SetActive(false);
        _director.gameObject.SetActive(true);
        scetch.transform.DOLocalMove(new Vector3(5.26621756f,0f,0f), 1f).SetEase(ease).SetRelative();
    }
    public void FadeMaterial()
    {
        for(int i=0;i<dissolvesMaterials.Length;i++)
        {
            Material material = dissolvesMaterials[i].material;
            DOTween.To(() => material.GetFloat("_DissolveAmount"),x => material.SetFloat("_DissolveAmount", x),1f,1f);
        }
    }
    public void StartSettingRoutine()
    {
        currentStage++;
        _director.gameObject.SetActive(false);
        stageSet[currentStage].viewPicture.GetComponentInChildren<Picture>().GetComponent<SpriteRenderer>().DOFade(1f, 0.2f);
        Debug.Log(stageSet[currentStage].viewPicture.GetComponentInChildren<Picture>().GetComponent<SpriteRenderer>().name);
        DOVirtual.DelayedCall(2f,()=> {
            StageNameEvent stageNameEvent = UIEvents.StageNameEvent;
            stageNameEvent.Text = stageSet[currentStage].stageName;
            stageNameEvent.duration = 3f;
            uiChannel.RaiseEvent(stageNameEvent);


            stageSet[currentStage].stage.SetActive(true);
            FindAnyObjectByType<Player>().GetCompo<EntityMover>().RbCompo.isKinematic = false;
            stageSet[currentStage].viewPicture.material = stageCamera;
            FindAnyObjectByType<Player>().ReStartSet(false, stageSet[currentStage].drawPictureName);

            for (int i = 0; i < dissolvesMaterials.Length; i++)
            {
                Material material = dissolvesMaterials[i].material;
                material.SetFloat("_DissolveAmount", 0f);
            }
            stageSet[currentStage].viewPicture.GetComponentInChildren<Picture>().GetComponent<SpriteRenderer>().DOFade(0f, 0.2f);
        });
    }
}

[Serializable]
public struct StageSet
{
    public string stageName;
    public MeshRenderer viewPicture;
    public GameObject stage;
    public string[] drawPictureName;
}
