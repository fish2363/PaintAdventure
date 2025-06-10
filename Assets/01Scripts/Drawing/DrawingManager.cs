using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class DrawManager : MonoBehaviour,IEntityComponent
{
    public Transform brush;
    public Animator brushAnimator;
    public Camera brushCamera;

    public GameObject drawingRenderer;
    public CinemachineCamera freeLook;
    public CinemachineCamera drawView;

    private Player _player;

    public static bool isDrawing;

    private bool isOnDraw;
    public string[] currentCanGestures;
    public StageSystem stageSystem;

    public void Initialize(Entity entity)
    {
        Cursor.visible = false;

        if (brushCamera != null)
            brushCamera.gameObject.SetActive(false);

        _player = entity as Player;
        _player.InputReader.OnDrawingEvent += HandleDrawBtn;
    }
    private void OnDestroy()
    {
        _player.InputReader.OnDrawingEvent -= HandleDrawBtn;
    }
    private void HandleDrawBtn(bool isHoldPencil)
    {
        if (isOnDraw || currentCanGestures.Length < 1) return;
        SetDrawingMode(isHoldPencil);
    }

    void Update()
    {
        BrushMovement();
        DrawingAnimation();
    }

    private void DrawingAnimation()
    {
        //if (brushAnimator != null && brushAnimator.gameObject.activeSelf)
        //{
        //    brushAnimator.SetFloat("X", Mathf.Lerp(brushAnimator.GetFloat("X"), Input.GetAxis("Mouse X") * 1, .09f));
        //    brushAnimator.SetFloat("Y", Mathf.Lerp(brushAnimator.GetFloat("Y"), Input.GetAxis("Mouse Y") * 1, .09f));
        //    brushAnimator.SetBool("isDrawing", Input.GetMouseButton(0));
        //}
    }

    private void BrushMovement()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = .4f;
        if (isDrawing)
            brush.position = Vector3.Lerp(brush.position, Camera.main.ScreenToWorldPoint(temp), 1f);
        ClampPosition(brush);
    }

    void ClampPosition(Transform obj)
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(obj.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        obj.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public void SetDrawingMode(bool state)
    {
        FindAnyObjectByType<UIManager>().KakaoTipOut();

        GestureShow gestureShow = UIEvents.GestureShow;
        gestureShow.gestureName = null;
        _player.UIChannel.RaiseEvent(gestureShow);

        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;

        isDrawing = state;
        SkillUIEvent skillUIEvent = UIEvents.SkillUIEvent;
        skillUIEvent.isHide = state;

        _player.UIChannel.RaiseEvent(skillUIEvent);
        freeLook.enabled = !state;
        drawView.enabled = state;

        StartCoroutine(ScaleWait(state));
        drawingRenderer.SetActive(state);
        brushCamera.gameObject.SetActive(state);

        //drawingRenderer.transform.DOLocalRotate(new Vector3(isDrawing ? 70 : 90, 180, 0), 0.5f, RotateMode.Fast).SetUpdate(true);
        //DOVirtual.Float(grainVolume.weight, effectAmount, 0.5f, (x) => grainVolume.weight = x).SetUpdate(true);
        //drawingRenderer.material.DOFloat(effectAmount, "SepiaAmount", 0.5f).SetUpdate(true);

        if (!state)
        {
            FindAnyObjectByType<GestureRecognizer>()?.TryRecognize(currentCanGestures);
        }
    }
    public void GoalScale()
    {
        FindAnyObjectByType<UIManager>().KakaoTipOut();
        isOnDraw = !isOnDraw;
        freeLook.enabled = !isOnDraw;
        drawView.enabled = isOnDraw;
        drawingRenderer.SetActive(isOnDraw);
        StartCoroutine(ScaleWait(isOnDraw));

        if (stageSystem.IsClear && isOnDraw) stageSystem.StageClear();
    }
    private IEnumerator ScaleWait(bool isEnabled)
    {
        drawingRenderer.transform.DOKill();
        drawingRenderer.SetActive(isEnabled);
        if (isEnabled)
        {
            yield return drawingRenderer.transform.DOScale(new Vector3(0.1050295f, 0.05834971f, 0.05834971f), 0.5f).SetUpdate(true)
                .WaitForCompletion();
        }
        else
            yield return drawingRenderer.transform.DOScale(new Vector3(0.1466158f, 0.08145322f, 0.08145322f), 0.5f);
    }

}

