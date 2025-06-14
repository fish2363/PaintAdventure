using Ami.BroAudio;
using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class WakeUpMonster : ExtendedMono
{
    public SoundID bossSound;
    public SoundID boomSound;
    [SerializeField] private SoundID stage1BGM;
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField] private Material[] monsterOtherMaterial;
    private bool isOneTime;
    [SerializeField] ParticleSystem d;
    public UnityEvent OnCutSceneEndEvent;
    Animator animator;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private CinemachineCamera cinemachine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        BroAudio.Play(stage1BGM);
        animator.Play("Laying");
    }
    public void BossSound()
    {
        BroAudio.Play(bossSound);
    }

    public void BoomSound()
    {
        BroAudio.Play(boomSound);
    }

    public void CameraShake()
        => Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, radius, whatIsPlayer) &&!isOneTime)
        {
            isOneTime = true;
            BroAudio.Stop(stage1BGM);
            FindAnyObjectByType<Player>().GetCompo<EntityMover>().CanManualMove = false;
            StartSafeCoroutine("WakeUpRoutine", WakeUpRoutine());
        }
    }
    public void ChangeFOV(float value)
    {
        DOTween.To(() => cinemachine.Lens.FieldOfView, x => cinemachine.Lens.FieldOfView = x, value, 0.2f);
    }
    private IEnumerator WakeUpRoutine()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[0];
        yield return new WaitForSeconds(1f);
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[2];
        yield return new WaitForSeconds(0.1f);
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[0];
        yield return new WaitForSeconds(0.1f);
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[2];
        yield return new WaitForSeconds(2f);
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[1];
        yield return new WaitForSeconds(2f);
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[0];
        yield return new WaitForSeconds(4f);
        director.Play();
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }
    public void ChangeAnimation(string anim)
    {
        animator.Play(anim);
    }
    public void CutSceneEnd()
    {
        FindAnyObjectByType<Player>().GetCompo<EntityMover>().CanManualMove = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        d.Play();
        FindAnyObjectByType<StageSystem>().IsClear = true;
        OnCutSceneEndEvent?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.white;
    }
}
