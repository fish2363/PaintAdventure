using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MovePlace : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveDuration = 5f;

    private Vector3 lastPosition;
    private Vector3 moveDelta;

    private Tweener moveTween;
    [SerializeField]
    private bool isMoving = false;
    private Player player;

    public UnityEvent OnSibal;
    public UnityEvent OnArrive;

    [field:SerializeField]
    public bool IsStop { get; set; }

    private bool isEnd;

    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform Middle;
    private bool isAlreadyMove;

    [SerializeField]
    private PushButton[] sibaaHardCodingbbbbbbbbbbbbbb;
    

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
        player.OnDeadEvent.AddListener(ComebackPoint);
    }
    private void OnDisable()
    {
        player.OnDeadEvent.AddListener(ComebackPoint);

    }

    private void Start()
    {
        transform.position = startPoint.position;
        lastPosition = transform.position;
    }
    
    private void FixedUpdate()
    {
        moveDelta = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("CanPush"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null && moveDelta != Vector3.zero)
            {
                rb.MovePosition(rb.position + moveDelta);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Player") && !isMoving || collision.gameObject.CompareTag("CanPush")) && isEnd == false)
        {
            player.GetCompo<EntityMover>().CanJump = false;
            MoveToEnd();
            OnSibal?.Invoke();
        }
    }

    public void MoveToEnd()
    {
        isMoving = true;
        Debug.Log("¤µ¤²");
        moveTween = transform.DOMove(endPoint.position, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            { isMoving = false;
                player.GetCompo<EntityMover>().CanJump = true;
                OnArrive?.Invoke();
                isEnd = true;
            });
    }

    public void MoveToValue(int direction)
    {
        if (isMoving) return;

        Transform move = isAlreadyMove ? SibalMiddle() : Direction(direction);


        isMoving = true;
        transform.DOMove(move.position, 1f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                if(isAlreadyMove)
                {
                    isAlreadyMove = false;
                    for (int i = 0; i < sibaaHardCodingbbbbbbbbbbbbbb.Length; i++)
                        sibaaHardCodingbbbbbbbbbbbbbb[i].Unlock();
                }
                else
                    isAlreadyMove = true;
                isMoving = false;
                player.GetCompo<EntityMover>().CanJump = true;
            });
    }
    private Transform SibalMiddle()
    {
        
        return Middle;
    }

    private Transform Direction(int direction)
    {
        return direction > 0 ? right : left;
    }

    public void StopPause()
    {
        PauseMovement(true,0);
    }
    public void PauseMovement(bool isStop, float duration = 0)
    {
        if (moveTween != null && moveTween.IsPlaying())
        {
            moveTween.Pause();
            if(!isStop)
            StartCoroutine(ResumeAfterDelay(duration));
        }
    }
    public void CancelPause() => moveTween.Play();

    private IEnumerator ResumeAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        moveTween.Play();
    }

    public void ComebackPoint()
    {
        moveTween?.Kill();
        transform.DOMove(startPoint.position, 1f).SetEase(Ease.Linear);
        isMoving = false;
    }
}
