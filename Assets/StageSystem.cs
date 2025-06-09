using DG.Tweening;
using System;
using System.Collections;
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

        StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
        startTipDialogueEvent.tipText = "좋아요. 우선\n지금까지 그리신 거 보면서 하나 하나\n피드백 해드릴게요";
        uiChannel.RaiseEvent(startTipDialogueEvent);


        FindAnyObjectByType<Player>().ReStartSet(false, stageSet[currentStage].drawPictureName);
    }

    [ContextMenu("StageClear")]
    public void StageClear()
    {
        IsClear = false;
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
