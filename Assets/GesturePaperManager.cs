using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GesturePaperManager : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GestureSOList GestureSOList;
    private Dictionary<string, GestureSO> gestureDictionary = new();
    [SerializeField] private GameObject gestureSlot;
    private CanvasGroup canvasGroup;
    private bool isHide;
    [SerializeField] private RectTransform selectGesture;
    [SerializeField]
    private HorizontalLayoutGroup grid;
    private List<GesturePaper> gestures = new();
    [SerializeField] private GameObject point;
    private List<Quaternion> originalRotations = new();

    private void Awake()
    {
        foreach(GestureSO gesture in GestureSOList.gestureSO)
        {
            gestureDictionary.Add(gesture.gestureFileName,gesture);
        }
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void HideUI(bool dd)
    {
        int value = dd ? 0 : 1;
        canvasGroup.alpha = value;
        isHide = !isHide;
    }

    public void SetGesturePicture(string[] name)
    {
        for(int i=0;i<gestures.Count;i++)
            Destroy(gestures[i].gameObject);
        gestures.Clear();
        originalRotations.Clear();
        float spreadAngle = -60f;
        float startAngle = 60f;
        float angleStep = spreadAngle / (name.Length - 1);

        for (int i = 0; i < name.Length; i++)
        {
            float angleZ = startAngle + angleStep * i;

            GameObject card = Instantiate(gestureSlot, grid.transform);
            GesturePaper paper = card.GetComponent<GesturePaper>();
            paper.ChangeImage(gestureDictionary[name[i]], this);
            gestures.Add(paper);

            Quaternion rotation = Quaternion.Euler(0, 0, -angleZ);
            card.transform.localRotation = rotation;

            originalRotations.Add(rotation);

        }
        grid.transform.DOLocalMove(transform.position, 0.1f);
        DOTween.To(()=> grid.spacing,x=> grid.spacing=x,-70f,1f);
    }
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        for(int i=0;i<gestures.Count;i++)
        {
            gestures[i].transform.DOLocalRotateQuaternion(Quaternion.identity, 0.5f);
        }
        Vector2 vector2 = new Vector2(Screen.width / 2, Screen.height / 2);
        grid.transform.DOMove(vector2, 1f);
        DOTween.To(() => grid.spacing, x => grid.spacing = x, -1250f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < gestures.Count; i++)
        {
            gestures[i].transform.DOLocalRotateQuaternion(originalRotations[i], 0.5f);
        }
        grid.transform.DOLocalMove(transform.position, 1f);
        DOTween.To(() => grid.spacing, x => grid.spacing = x, -70f, 1f);
    }
}
