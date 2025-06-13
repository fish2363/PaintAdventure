using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ESC : MonoBehaviour
{
    [SerializeField]
    private InputReader Input;
    [SerializeField] private VideoPlayer video;
    private bool isOn;
    private bool isPlayVideo;
    private bool isWaitEnd;
    private float videoPlayTime;
    [SerializeField]
    private RawImage paperVideo;

    private void Awake()
    {
        Input.OnESCKeyEvent += EscKeyHandle;
    }
    private void OnDestroy()
    {
        Input.OnESCKeyEvent -= EscKeyHandle;
    }
    private void Update()
    {
        if (!video.isPlaying && isWaitEnd)
        {
            video.time = 0f;
            isWaitEnd = false;
        }
        if (video.time >= 3f && isPlayVideo)
        {
            isPlayVideo = false;
            isWaitEnd = false;
            video.Pause();
            paperVideo.transform.DOScale(1.876956f, 0.2f).SetEase(Ease.InBack)
                .OnComplete(()=> Time.timeScale = 0f);
        }
    }
    private void EscKeyHandle()
    {
        if (isWaitEnd) return;
        if(!isOn)
        {
            paperVideo.DOFade(1f, 0.1f);
            paperVideo.transform.DOScale(0.9963667f, 0.2f);
            isWaitEnd = true;
            isPlayVideo = true;
            video.Play();
        }
        else
        {
            paperVideo.transform.DOScale(0.9963667f, 0.2f);
            isPlayVideo = false;
            isWaitEnd = true;
            video.Play();
            DOVirtual.DelayedCall(1.2f, () => {
            paperVideo.transform.DOScale(0f, 0.3f);
            Time.timeScale = 1f;
            });
        }
        isOn = !isOn;
    }
}
