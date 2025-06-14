using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Ami.BroAudio;
using System.Collections;
using UnityEditor;

public class ESC : MonoBehaviour
{
    public static ESC Instance;

    [SerializeField] private BroAudioType _bgm;
    [SerializeField] private BroAudioType _sfx;
    [SerializeField] private BroAudioType _main;

    [SerializeField]
    private InputReader Input;
    [SerializeField] private VideoPlayer video;
    public bool isOn;
    private bool isPlayVideo;
    private bool isWaitEnd;

    [SerializeField] private RawImage paperVideo;
    [SerializeField] private RawImage exitVideo;

    [SerializeField]
    private Image[] escPanel;
    public CanvasGroup escCanvas;
    public CanvasGroup exitCanvas;
    [SerializeField] private VideoPlayer exitPlay;
    public bool isFake;

    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Awake()
    {
        if (isFake) return;
        Input.OnESCKeyEvent += EscKeyHandle;


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        if (isFake) return;
        Input.OnESCKeyEvent -= EscKeyHandle;
    }
    public void SetEscPanel(int value)
    {
        for (int i = 0; i < escPanel.Length; i++)
        {
            if (i == value){
                Debug.Log(escPanel[i].name);
                escPanel[i].gameObject.SetActive(true);
            }
            else escPanel[i].gameObject.SetActive(false);
        }
    }

    public void BGM(float volume)
    {
        _bgmSlider.value = volume;
        BroAudio.SetVolume(_bgm, volume);
    }

    public void SFX(float volume)
    {
        _sfxSlider.value = volume;
        BroAudio.SetVolume(_sfx, volume);
    }

    public void Master(float volume)
    {
        _masterSlider.value = volume;
        BroAudio.SetVolume(_main, volume);
    }
    public void ExitVieoStart()
    {
        Time.timeScale = 1f;
        exitVideo.DOFade(1f, 0.2f).OnComplete(() => exitPlay.Play());
    }
    public void ShowEXITCanvas()
    {
        exitCanvas.alpha = 1f;
        exitCanvas.interactable = true;
        exitCanvas.blocksRaycasts = true;
    }
    public void HideEXITCanvas()
    {
        exitCanvas.alpha = 0f;
        exitCanvas.interactable = false;
        exitCanvas.blocksRaycasts = false;
    }
    private void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 에디터 실행 중지
#else
        Application.Quit(); // 빌드된 게임 종료
#endif
    }
    private void OnDestroy()
    {
        if (isFake) return;
        Input.OnESCKeyEvent -= EscKeyHandle;
    }
    private void Update()
    {
        if (isFake) return;
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
                    escCanvas.blocksRaycasts = true;
                    Time.timeScale = 0f;
                });
        }
        if (exitPlay.time >= 2f)
        {
            exitPlay.Pause();
            Exit();
        }
    }
    public void EscKeyHandle()
    {
        if (isFake) return;
        if (isWaitEnd) return;
        if(!isOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
                escCanvas.blocksRaycasts = false;
                paperVideo.transform.DOScale(0f, 0.3f);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            });
        }
        isOn = !isOn;
    }
}
