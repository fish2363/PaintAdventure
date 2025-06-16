using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using Unity.Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour
{
    [SerializeField]
    private GameEventChannelSO uiChannel;

    public Transform pointA;
    public Transform pointB;
    public Transform player;           
    public float moveDuration = 2f;
    public float waitAtPointTime = 2f;
    public float curveHeight = 2f;
    public float idleTime = 10f;
    public float attackInterval = 30f;
    public float lookSpeed = 5f;         // 회전 속도


    public GameObject bulletPrefab;   // 총알 프리팹
    public Transform firePoint;       // 발사 위치
    public float bulletSpeed = 10f;   // 총알 속도

    public string damageTag = "PlayerAttack";     // 충돌 감지할 태그
    private int currentHits = 3;                  // 현재 맞은 횟수
    private int plateHits = 3;                  // 현재 맞은 횟수

    private bool isDead = false;    
    private bool isDamage = false;    

    private float lastAttackTime;
    private bool movingToB = true;
    private float moveTimer = 0f;
    private bool isWaitingAtPoint = false;

    private bool isRotation = false;

    private enum BossState { Idle, Attacking }
    private BossState currentState = BossState.Idle;

    private Coroutine currentCoroutine;
    private Animator animator;
    [SerializeField]
    private ParticleSystem danger;
    [SerializeField]
    private ParticleSystem hitParticle;
    [SerializeField] private CinemachineCamera cinemachine;

    [SerializeField]
    private TextMeshPro hpText;
    [SerializeField] private Image panel;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lastAttackTime = Time.time;
        currentCoroutine = StartCoroutine(IdleRoutine());
    }
    public void Damage(GameObject bomb)
    {
        if (isDead) return;
        isDamage = true;
        --currentHits;
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        Destroy(bomb);
        hitParticle.Play();

        StartCoroutine(DieAndPlayCutscene());
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("MoveFlatform"))
        {
            BossCollisionFlatform();
        }
    }
    public void BossUI()
    {
        QuestEvent questEvnet = UIEvents.QuestEvent;
        questEvnet.text = "풍선으로 폭탄을 날려서 공격하세요\n바닥이 부서지지 않게 지키세요";
        questEvnet.isClear = false;
        questEvnet.duration = 3f;
        uiChannel.RaiseEvent(questEvnet);
    }

    public void BossCollisionFlatform()
    {
        --plateHits;
        hpText.text = $"{plateHits}";
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        DOTween.To(() => cinemachine.Lens.Dutch, x => cinemachine.Lens.Dutch = x, 180f, 1f);
        DOVirtual.DelayedCall(2f,()=> DOTween.To(() => cinemachine.Lens.Dutch, x => cinemachine.Lens.Dutch = x, 0f, 1f));
    }
    private IEnumerator DieAndPlayCutscene()
    {
        if (animator != null)
        animator.Play("Armature_Dollgin"); // 정지 상태로 대기


        // 현재 움직임/코루틴 정지
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        yield return new WaitForSeconds(3f); // 약간의 딜레이

        if (currentHits <= 0)
        {
            Debug.Log("보스 사망. 컷씬 시작");

            isDead = true;
            yield return new WaitForSeconds(0.5f); // 약간의 딜레이

            panel.DOFade(1f,3f).OnComplete(()=>SceneManager.LoadScene("Ending"));
        }
        else
            currentState = BossState.Idle;
        isDamage = false;
    }
    private void Update()
    {
        if (isDead || isDamage) return; // 죽었으면 아무것도 안 함

        if (currentState == BossState.Idle)
        {
            if (!isWaitingAtPoint)
                MoveAlongCurve();
        }

        if (player != null)
            LookAtPlayer();

        if (currentState == BossState.Idle && Time.time - lastAttackTime >= attackInterval)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    private void LookAtPlayer()
    {
        if (isRotation) return;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // 수직 회전은 무시

        if (direction.magnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion offset = Quaternion.Euler(0, 90f, 0); // 필요에 따라 -90도나 다른 값으로 조절

            Quaternion finalRotation = lookRotation * offset;
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * lookSpeed);
        }
    }

    private void MoveAlongCurve()
    {
        if (animator != null)
            animator.Play("Armature_IDLE");

        moveTimer += Time.deltaTime;
        float t = moveTimer / moveDuration;
        t = Mathf.Clamp01(t);

        Vector3 start = movingToB ? pointA.position : pointB.position;
        Vector3 end = movingToB ? pointB.position : pointA.position;
        Vector3 flatPos = Vector3.Lerp(start, end, t);

        float curveOffset = Mathf.Sin(t * Mathf.PI) * curveHeight;
        flatPos.y += curveOffset;

        transform.position = flatPos;

        if (t >= 1f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    private IEnumerator WaitAtPoint()
    {
        isWaitingAtPoint = true;

        yield return new WaitForSeconds(waitAtPointTime);

        movingToB = !movingToB;
        moveTimer = 0f;
        isWaitingAtPoint = false;
    }

    private IEnumerator IdleRoutine()
    {
        currentState = BossState.Idle;
        float timer = 0f;

        while (timer < idleTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        currentState = BossState.Attacking;

        Debug.Log("보스 공격 시작");

        int patternIndex = Random.Range(0, 2);
        switch (patternIndex)
        {
            case 0:
                currentCoroutine = StartCoroutine(Pattern1());
                yield return currentCoroutine;
                break;
            case 1:
                currentCoroutine = StartCoroutine(Pattern2());
                yield return currentCoroutine;
                break;
        }

        Debug.Log("보스 공격 종료");
        currentCoroutine = null;

        lastAttackTime = Time.time;
        currentCoroutine = StartCoroutine(IdleRoutine());
    }

    private IEnumerator Pattern1()
    {
        isRotation = true;
        int patternIndex = Random.Range(0, 2);
        Transform movePoint = patternIndex == 0 ? pointA : pointB;
        transform.DOMove(movePoint.position, 1f);

        animator.Play("Armature_Dollgin"); // 정지 상태로 대기
        danger.Play();
        yield return new WaitForSeconds(4.3f);
        danger.Stop();

        Quaternion lookRotation = Quaternion.LookRotation(Vector3.down);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
        yield return new WaitForSeconds(1f);

        if (player != null)
        {
            float dashDistance = 50f;
            float dashSpeed = 20f;
            Vector3 targetPos = transform.position + Vector3.down * dashDistance;

            float dashTimer = 0f;
            float dashDuration = dashDistance / dashSpeed;

            if (animator != null)
                animator.Play("Armature_AWAKE");

            while (dashTimer < dashDuration)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, dashTimer / dashDuration);
                dashTimer += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
        }

        yield return new WaitForSeconds(0.3f); // 후 정리 시간
        isRotation = false;
    }

    private IEnumerator Pattern2()
    {
        // Step 1: 기 모으는 시간
        if (animator != null)
            animator.Play("Armature_Shoot");
        yield return new WaitForSeconds(2f);

        Vector3 dir = (player.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = dir * bulletSpeed;

        yield return new WaitForSeconds(0.3f); // 후 정리 시간
    }
}