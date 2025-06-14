using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class BossAI : MonoBehaviour
{
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
    public int maxHits = 3;                       // 최대 맞을 수 있는 횟수
    private int currentHits = 0;                  // 현재 맞은 횟수
    public PlayableDirector cutsceneDirector;     // 연결할 Timeline

    private bool isDead = false;    

    private float lastAttackTime;
    private bool movingToB = true;
    private float moveTimer = 0f;
    private bool isWaitingAtPoint = false;

    private enum BossState { Idle, Attacking }
    private BossState currentState = BossState.Idle;

    private Coroutine currentCoroutine;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lastAttackTime = Time.time;
        currentCoroutine = StartCoroutine(IdleRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag(damageTag))
        {
            currentHits++;
            Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            Destroy(other);
            if (currentHits >= maxHits)
            {
                StartCoroutine(DieAndPlayCutscene());
            }
        }
    }
    private IEnumerator DieAndPlayCutscene()
    {
        isDead = true;

        if (animator != null)
            animator.Play("Armature_IDLE");

        // 현재 움직임/코루틴 정지
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentState = BossState.Idle;

        Debug.Log("보스 사망. 컷씬 시작");

        yield return new WaitForSeconds(0.5f); // 약간의 딜레이

        // Timeline 재생
        if (cutsceneDirector != null)
            cutsceneDirector.Play();
    }
    private void Update()
    {
        if (isDead) return; // 죽었으면 아무것도 안 함

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
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // 수직 회전은 무시

        if (direction.magnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
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

        if (animator != null) animator.SetBool("isMoving", false);

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

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetTrigger("attack");
        }

        Debug.Log("보스 공격 시작");

        int patternIndex = Random.Range(0, 2);
        switch (patternIndex)
        {
            case 0:
                yield return StartCoroutine(Pattern1());
                break;
            case 1:
                yield return StartCoroutine(Pattern2());
                break;
        }

        Debug.Log("보스 공격 종료");

        lastAttackTime = Time.time;
        currentCoroutine = StartCoroutine(IdleRoutine());
    }

    private IEnumerator Pattern1()
    {
        if (animator != null)
            animator.Play("Armature_IDLE"); // 정지 상태로 대기
        yield return new WaitForSeconds(2f);

        // Step 2: 살짝 뒤로 이동 (부드럽게)
        Vector3 backDir = -transform.forward;
        Vector3 startPos = transform.position;
        Vector3 backPos = startPos + backDir * 2f; // 뒤로 2미터

        float backTime = 0.3f;
        float timer = 0f;
        while (timer < backTime)
        {
            transform.position = Vector3.Lerp(startPos, backPos, timer / backTime);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = backPos;

        yield return new WaitForSeconds(0.1f); // 살짝 텀 주기

        // Step 3: 플레이어 방향으로 돌진
        if (player != null)
        {
            Vector3 dashDir = (player.position - transform.position).normalized;
            float dashDistance = 10f;
            float dashSpeed = 20f;
            Vector3 targetPos = transform.position + dashDir * dashDistance;

            float dashTimer = 0f;
            float dashDuration = dashDistance / dashSpeed;

            if (animator != null)
                animator.Play("Armature_Dollgin");

            while (dashTimer < dashDuration)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, dashTimer / dashDuration);
                dashTimer += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
        }

        yield return new WaitForSeconds(0.3f); // 후 정리 시간
    }

    private IEnumerator Pattern2()
    {
        // Step 1: 기 모으는 시간
        if (animator != null)
            animator.Play("Armature_Shoot");
        yield return new WaitForSeconds(3f);

        // Step 2: 총알 2개 발사
        if (player != null && bulletPrefab != null && firePoint != null)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.linearVelocity = dir * bulletSpeed;

                yield return new WaitForSeconds(0.5f); // 총알 사이 간격
            }
        }

        yield return new WaitForSeconds(0.3f); // 후 정리 시간
    }
}