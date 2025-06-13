using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSceneLogic : MonoBehaviour
{
    [SerializeField]
    private Image _black;
    [SerializeField]
    private TextMeshProUGUI youCanSkipText;

    private float skipGage;
    [SerializeField]
    private Image skipGageBar;
    [SerializeField]
    private Image pauseImage;

    [SerializeField]
    private VideoPlayer introVideo;
    [SerializeField] private Transform kakaoUI;
    [SerializeField] private Transform kakaoInPos;
    [SerializeField] private Transform kakaoOutPos;
    [SerializeField] private TextMeshProUGUI kakaoText;
    [SerializeField] private string[] talks;
    private int currentTalk;
    private bool isVideoEnd;

    private bool isClick;
    private bool isWaitClick;

    private void Start()
    {
        _black.DOFade(0f,0.5f);
        StartCoroutine(WaitRoutine());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) &&isWaitClick)
        {
            isClick = true;
        }
        if (!isVideoEnd)
        {
            skipGageBar.GetComponent<RectTransform>().localScale = new Vector3(skipGage, skipGageBar.transform.localScale.y, 0);
            if (Input.GetKey(KeyCode.Space))
            {
                if (skipGage > 8)
                {
                    PauseEvent();
                    skipGageBar.gameObject.SetActive(false);
                }
                else
                {
                    skipGage += Time.deltaTime * 2;
                    skipGageBar.GetComponent<Image>().DOFade(1, skipGage);
                }
            }
            else
            {
                if (skipGage > 0)
                {
                    skipGage -= Time.deltaTime * 3;
                    skipGageBar.GetComponent<Image>().DOFade(0, skipGage);
                }
            }

            if (introVideo.time > 19.2f)
            {
                PauseEvent();
            }
        }
    }

    private void PauseEvent()
    {
        introVideo.Stop();
        isVideoEnd = true;
        pauseImage.gameObject.SetActive(true);
        StartCoroutine(KakaoTalk());
    }
    public IEnumerator KakaoTalk()
    {
        while(true)
        {
            if (currentTalk >= talks.Length) break;
            kakaoText.text = talks[currentTalk];
            kakaoUI.DOMove(kakaoInPos.position, 1f).WaitForCompletion();
            yield return new WaitUntil(()=>DelayWaitClick());
            kakaoUI.DOMove(kakaoOutPos.position, 0.5f).WaitForCompletion();
            currentTalk++;
        }
        SceneManager.LoadScene("TestKYH");
    }

    public bool DelayWaitClick()
    {
        isWaitClick = true;
        if (isClick)
        {
            isWaitClick = false;
            isClick = false;
            return true;
        }
        else return false;
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(1f);
        youCanSkipText.DOFade(1, 1);
        yield return new WaitForSeconds(10f);
        youCanSkipText.DOFade(0, 1);
    }
}
