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
    
    [ContextMenu("StageClear")]
    public void StageClear()
    {
        IsClear = false;
        FindAnyObjectByType<Player>().GetCompo<EntityMover>().RbCompo.isKinematic = true;
        stageSet[currentStage].stage.SetActive(false);
        ++currentStage;
        _director.gameObject.SetActive(true);
        scetch.transform.DOLocalMove(new Vector3(5.26621756f,0f,0f), 1f).SetEase(ease).SetRelative();
    }

    public void StartSettingRoutine()
    {
        StageNameEvent stageNameEvent = UIEvents.StageNameEvent;
        stageNameEvent.Text = stageSet[currentStage].stageName;
        stageNameEvent.duration = 3f;
        uiChannel.RaiseEvent(stageNameEvent);


        stageSet[currentStage].stage.SetActive(true);
        FindAnyObjectByType<Player>().GetCompo<EntityMover>().RbCompo.isKinematic = false;
        stageSet[currentStage].viewPicture.material = stageCamera;
        _director.gameObject.SetActive(false);
        FindAnyObjectByType<Player>().ReStartSet(stageSet[currentStage].drawPictureName);
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
