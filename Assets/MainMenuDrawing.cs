using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDrawing : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    public List<Transform> movePos;
    public Transform pencil;
    private int currentIdx;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            currentIdx++;
            if (currentIdx >= movePos.Count) currentIdx = 0;
            Debug.Log(currentIdx);
            pencil.DOMove(movePos[currentIdx].position,0.4f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentIdx--;
            if (currentIdx < 0) currentIdx = movePos.Count-1;
            pencil.DOMove(movePos[currentIdx].position, 0.4f);
        }
    }
}
