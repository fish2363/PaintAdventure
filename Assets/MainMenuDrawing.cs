using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDrawing : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    public List<Transform> movePos;
    public Transform pencil;
    private int currentIdx;
    public Ease pushLeft;
    public Ease pushRight;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            currentIdx++;
            if (currentIdx >= movePos.Count) currentIdx = 0;
            pencil.DORotate(new Vector3(0f, pencil.rotation.y - 90f * currentIdx, 0f), 0.7f).SetRelative();
            Debug.Log(currentIdx);
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
