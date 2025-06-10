using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GesturePaper : MonoBehaviour
{
    public Image imagur;
    public TextMeshProUGUI text;

    public void ChangeImage(GestureSO gesture)
    {
        imagur.sprite = gesture.gestureImage;
        text.text = gesture.gestureName;
    }
}
