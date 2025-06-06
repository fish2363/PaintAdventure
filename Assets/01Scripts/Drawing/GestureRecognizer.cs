using DG.Tweening;
using PDollarGestureRecognizer;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
    private Rect drawArea;

    private List<Gesture> trainingSet = new List<Gesture>();

    public bool IsDrawArea => drawArea.Contains(Input.mousePosition);

    private bool recognized;
    private int vertexCount = 0;

    private List<Point> points = new List<Point>();
    private int strokeId = -1;
    public Transform gestureOnScreenPrefab;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;

    public LayerMask layerMask;
    public Transform balloonPrefab;
    public Transform chickPrefab;
    public GameObject cubePrefab;

    public GameObject drawingHand;
    public GameObject moveHand;

    private void Start()
    {
        drawArea = new Rect(0, 0, Screen.width, Screen.height);

        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        //Load user custom gestures
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
    }

    private void Update()
    {
        if (DrawManager.isDrawing)
        {
            if (recognized)
            {
                recognized = false;
                ClearLine();
            }
            if (Input.GetMouseButtonDown(0))
            {

                moveHand.SetActive(false);
                drawingHand.SetActive(true);

                ++strokeId;
                Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
                //Selection.activeGameObject = tmpGesture.gameObject;

                gestureLinesRenderer.Add(currentGestureLineRenderer);

                vertexCount = 0;
            }
            if(Input.GetMouseButtonUp(0))
            {
                drawingHand.SetActive(false);
                moveHand.SetActive(true);
            }
            if (Input.GetMouseButton(0))
            {
                points.Add(new Point(Input.mousePosition.x, Input.mousePosition.y, strokeId));

                currentGestureLineRenderer.positionCount = ++vertexCount;
                currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
            }
        }
    }

    public void TryRecognize()
    {
        if (points.Count == 0)
        {
            Debug.Log("안 그리는 중");
            return;
        }

        if (recognized)
            ClearLine();

        recognized = true;

        Gesture candidate = new Gesture(points.ToArray());

        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        if (gestureResult.Score < .75f)
        {
            ClearLine();
            return;
        }

        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        if(gestureResult.GestureClass == "MetalPlate")
        {
            Debug.Log("시바");
            GameObject b = Instantiate(cubePrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);

            ClearLine();
        }
        if (gestureResult.GestureClass == "ChickSummon")
        {
            Transform b = Instantiate(chickPrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.DOScale(0, .2f).From().SetEase(Ease.OutBack);

            ClearLine();
        }

        if (gestureResult.GestureClass == "ObjBalloon")
        {
            Debug.Log("슝");
            RaycastHit hit = new RaycastHit();

            Vector3 direction = (gestureLinesRenderer[0].bounds.center - Camera.main.transform.position).normalized;

            if (Physics.SphereCast(Camera.main.transform.position, 3f, direction, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log(hit.collider.name);
                Transform b = Instantiate(balloonPrefab, hit.collider.transform);
                b.DOScale(0, .2f).From().SetEase(Ease.OutBack);

                ClearLine();
            }

        }

        //if (gestureResult.GestureClass == "mixandjam")
        //{
        //	Transform b = Instantiate(jammoPrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity); ;
        //	b.DOScale(0, .2f).From().SetEase(Ease.OutBack);

        //	if (recognized)
        //	{
        //		recognized = false;
        //		strokeId = -1;

        //		points.Clear();

        //		foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        //		{
        //			lineRenderer.SetVertexCount(0);
        //			Destroy(lineRenderer.gameObject);
        //		}
        //		gestureLinesRenderer.Clear();
        //	}
        //}


        //if (gestureResult.GestureClass == "horizontal line" || gestureResult.GestureClass == "line")
        //{
        //    //loc = Vector3.MoveTowards(gestureLinesRenderer[0].bounds.center, Camera.main.transform.position, 5);

        //    RaycastHit hit = new RaycastHit();
        //    if (Physics.SphereCast(gestureLinesRenderer[0].bounds.center, 3, Camera.main.transform.forward, out hit, 15, layerMask))
        //    {
        //        if (hit.collider.CompareTag("Bridge"))
        //        {
        //            hit.collider.GetComponent<MeshRenderer>().enabled = true;
        //            Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        //        }
        //        else if (hit.collider.CompareTag("Cuttable"))
        //        {
        //            hit.collider.GetComponent<TreeScript>().Slash();
        //            Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        //        }
        //    }
    }


    private void OnDrawGizmos()
    {
        if (gestureLinesRenderer.Count < 1) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position, gestureLinesRenderer[0].bounds.center - Camera.main.transform.position);
        Gizmos.color = Color.white;
    }

    public void ClearLine()
    {
        strokeId = -1;

        points.Clear();

        foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        {
            lineRenderer.positionCount = 0;
            Destroy(lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear();
    }
}
