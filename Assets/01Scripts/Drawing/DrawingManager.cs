using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class DrawManager : MonoBehaviour,IEntityComponent
{
    public Transform brush;
    public Animator brushAnimator;
    public Camera brushCamera;

    public Renderer drawingRenderer;
    public CinemachineCamera freeLook;
    public CinemachineCamera drawView;

    private Player _player;

    public static bool isDrawing;

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
        SetDrawingMode(isHoldPencil);
    }

    void Update()
    {
        BrushMovement();
        DrawingAnimation();
    }

    private void DrawingAnimation()
    {
        if (brushAnimator != null && brushAnimator.gameObject.activeSelf)
        {
            brushAnimator.SetFloat("X", Mathf.Lerp(brushAnimator.GetFloat("X"), Input.GetAxis("Mouse X") * 1, .09f));
            brushAnimator.SetFloat("Y", Mathf.Lerp(brushAnimator.GetFloat("Y"), Input.GetAxis("Mouse Y") * 1, .09f));
            brushAnimator.SetBool("isDrawing", Input.GetMouseButton(0));
        }
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
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        isDrawing = state;
        Time.timeScale = isDrawing ? 0 : 1;

        freeLook.enabled = !state;
        drawView.enabled = state;

        StartCoroutine(ScaleWait(state));
        drawingRenderer.transform.GetChild(0).gameObject.SetActive(state);
        brushCamera.gameObject.SetActive(state);

        //drawingRenderer.transform.DOLocalRotate(new Vector3(isDrawing ? 60 : 90, 180, 0), 0.5f, RotateMode.Fast).SetUpdate(true);
        //DOVirtual.Float(grainVolume.weight, effectAmount, 0.5f, (x) => grainVolume.weight = x).SetUpdate(true);
        //drawingRenderer.material.DOFloat(effectAmount, "SepiaAmount", 0.5f).SetUpdate(true);

        if (!state)
        {
            FindObjectOfType<GestureRecognizer>()?.TryRecognize();
        }
    }

    private IEnumerator ScaleWait(bool isEnabled)
    {
        drawingRenderer.transform.DOKill();
        drawingRenderer.enabled = isEnabled;
        if (isEnabled)
        {
            yield return drawingRenderer.transform.DOScale(new Vector3(0.06f, 0.04045355f, 0.034f), 0.5f).SetUpdate(true)
                .WaitForCompletion();
        }
        else
            yield return drawingRenderer.transform.DOScale(new Vector3(0.07281639f, 0.04045355f, 0.04045355f), 0.5f);
    }

}

