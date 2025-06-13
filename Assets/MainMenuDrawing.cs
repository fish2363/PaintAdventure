using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuDrawing : MonoBehaviour
{
    public List<Transform> movePos;
    public Transform pencil;
    private int currentIdx;
    public Ease pushLeft;
    public Ease pushRight;
    public CinemachineCamera startCamera;
    public CinemachineCamera escCamera;
    public CinemachineCamera exitCamera;

    [SerializeField]
    private Image _black;

    private bool isChooseWait;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace) && isChooseWait)
        {
            escCamera.gameObject.SetActive(false);
            exitCamera.gameObject.SetActive(false);
            isChooseWait = false;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (movePos[currentIdx].name == "Setting")
            {
                isChooseWait = true;
                escCamera.gameObject.SetActive(true);
            }
            if(movePos[currentIdx].name == "Exit")
            {
                isChooseWait = true;
                exitCamera.gameObject.SetActive(true);
            }
            if (movePos[currentIdx].name =="Start")
            {
                startCamera.gameObject.SetActive(true);

                Sequence seq = DOTween.Sequence();

                seq.Append(DOTween.To(
                    () => startCamera.Lens.FieldOfView,
                    x =>
                    {
                        var lens = startCamera.Lens;
                        lens.FieldOfView = x;
                        startCamera.Lens = lens;
                    },
                    50f,
                    1f
                )).Append(DOTween.To(
                    () => startCamera.Lens.FieldOfView,
                    x =>
                    {
                        var lens = startCamera.Lens;
                        lens.FieldOfView = x;
                        startCamera.Lens = lens;
                    },
                    0f,
                    1f
                )).Join(_black.DOFade(1f, 1f))
                .Join(DOTween.To(
                    () => startCamera.Lens.Dutch,
                    x =>
                    {
                        var lens = startCamera.Lens;
                        lens.Dutch = x;
                        startCamera.Lens = lens;
                    },
                    360f,
                    2f))
                    .AppendCallback(() => SceneManager.LoadScene("CutScene"));

                DOVirtual.DelayedCall(2f,() =>
                {
                    seq.Play();
                });
            }
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            currentIdx++;
            if (currentIdx >= movePos.Count) currentIdx = 0;
            pencil.DORotate(new Vector3(0f, pencil.rotation.y - 90f * currentIdx, 0f), 0.7f).SetRelative();
            pencil.DOMove(movePos[currentIdx].position,0.4f).SetEase(pushLeft);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentIdx--;
            if (currentIdx < 0) currentIdx = movePos.Count-1;
            pencil.DORotate(new Vector3(0f, pencil.rotation.y + 90f * currentIdx, 0f), 0.7f).SetRelative();
            pencil.DOMove(movePos[currentIdx].position, 0.4f).SetEase(pushRight);
        }
    }
}
