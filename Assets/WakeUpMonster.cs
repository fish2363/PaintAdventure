using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WakeUpMonster : ExtendedMono
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField] private Material[] monsterOtherMaterial;
    private bool isOneTime;
    [SerializeField] ParticleSystem d;
    public UnityEvent OnCutSceneEndEvent;

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, radius, whatIsPlayer) &&!isOneTime)
        {
            isOneTime = true;
            FindAnyObjectByType<Player>().GetCompo<EntityMover>().CanManualMove = false;
            StartSafeCoroutine("WakeUpRoutine", WakeUpRoutine());
        }
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
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[1];
        yield return new WaitForSeconds(2f);
        GetComponentInChildren<SkinnedMeshRenderer>().material = monsterOtherMaterial[0];
        yield return new WaitForSeconds(2f);
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
