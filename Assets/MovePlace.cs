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
    private bool isMoving = false;
    private Player player;

    public UnityEvent OnSibal;

    [field:SerializeField]
    public bool IsStop { get; set; }

    private void OnEnable()
    {
      player =   FindAnyObjectByType<Player>();
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
        if (collision.gameObject.CompareTag("Player"))
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
        if (collision.gameObject.CompareTag("Player") && !isMoving)
        {
            MoveToEnd();
            OnSibal?.Invoke();
        }
    }

    public void MoveToEnd()
    {
        isMoving = true;
        moveTween = transform.DOMove(endPoint.position, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => isMoving = false);
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
