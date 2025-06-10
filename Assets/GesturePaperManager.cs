using System.Collections.Generic;
using UnityEngine;

public class GesturePaperManager : MonoBehaviour
{
    public GestureSOList GestureSOList;
    private Dictionary<string, GestureSO> gestureDictionary = new();
    [SerializeField] private GameObject gestureSlot;
    private CanvasGroup canvasGroup;
    private bool isHide;
    private void Awake()
    {
        foreach(GestureSO gesture in GestureSOList.gestureSO)
        {
            gestureDictionary.Add(gesture.gestureFileName,gesture);
        }
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void HideUI()
    {
        int value = isHide ? 0 : 1;
        canvasGroup.alpha = value;
        isHide = !isHide;
    }
    public void SetGesturePicture(string[] name)
    {
        for(int i=0;i<name.Length;i++)
        {
            GameObject card = Instantiate(gestureSlot,transform);
            card.GetComponent<GesturePaper>().ChangeImage(gestureDictionary[name[i]]);
            card.transform.localPosition = Vector3.zero;
        }
    }
}
