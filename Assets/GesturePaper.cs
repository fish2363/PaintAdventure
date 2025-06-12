using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GesturePaper : MonoBehaviour
{
    public Image imagur;
    public TextMeshProUGUI text;
    private GesturePaperManager paperManager;

    public void ChangeImage(GestureSO gesture,GesturePaperManager manager)
    {
        paperManager = manager;
        imagur.sprite = gesture.gestureImage;
        text.text = gesture.gestureName;
    }

}
