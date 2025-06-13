using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Ami.BroAudio;

public class ESC : MonoBehaviour
{
    public static ESC Instance;

    [SerializeField] private BroAudioType _bgm;
    [SerializeField] private BroAudioType _sfx;
    [SerializeField] private BroAudioType _main;

    [SerializeField]
    private InputReader Input;
    [SerializeField] private VideoPlayer video;
    private bool isOn;
    private bool isPlayVideo;
    private bool isWaitEnd;
    private float videoPlayTime;
    [SerializeField]
    private RawImage paperVideo;
    [SerializeField]
    private Image[] escPanel;
    public CanvasGroup escCanvas;

    private void Awake()
    {
        Input.OnESCKeyEvent += EscKeyHandle;

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetEscPanel(int value)
    {
        for(int i=0;i<escPanel.Length;i++)
        {
            if (i == value) escPanel[i].gameObject.SetActive(true);
            else escPanel[i].gameObject.SetActive(false);
        }
    }

    public void BGM(float volume)
    {
        BroAudio.SetVolume(_bgm, volume);
    }

    public void SFX(float volume)
    {
        BroAudio.SetVolume(_sfx, volume);
    }

    public void Master(float volume)
    {
        BroAudio.SetVolume(_main, volume);
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
                .OnComplete(()=>
                {
                    escCanvas.interactable = true;
                    escCanvas.alpha = 1f;
                    Time.timeScale = 0f;
                });
        }
    }
    public void EscKeyHandle()
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
                escCanvas.alpha = 0f;
                escCanvas.interactable = false;
                paperVideo.transform.DOScale(0f, 0.3f);
            Time.timeScale = 1f;
            });
        }
        isOn = !isOn;
    }
}
