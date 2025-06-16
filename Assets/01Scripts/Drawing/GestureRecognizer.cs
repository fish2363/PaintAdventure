using Ami.BroAudio;
using DG.Tweening;
using PDollarGestureRecognizer;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class GestureRecognizer : MonoBehaviour
{
    [SerializeField]
    private InputReader reader;

    private Rect drawArea;
    public SoundID drawSound;

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
    public LayerMask whatIsDrawArea;
    [Header("그림 소환 프리펩")]
    [SerializeField] private Transform balloonPrefab;
    [SerializeField] private Transform chickPrefab;
    [SerializeField] private GameObject ironPlatePrefab;
    [SerializeField] private GameObject carrotPrefab;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject pepperPrefab;
    [SerializeField] private GameObject flowerObject;
    [SerializeField] private GameObject treeObject;
    private GameObject currentBridgeObject;
    private GameObject metalObj;


    [Header("손")]
    [SerializeField] private GameObject drawingHand;
    [SerializeField] private GameObject moveHand;

    [Header("연출")]
    [SerializeField] private GameObject floatText;
    [SerializeField] private GameObject pointerPrefab;
    private GameObject pointerObject;

    [Header("소리")]
    [SerializeField] private SoundID bridgeInitSound;


    public static bool isOnPanel;
    private void Awake()
    {
        
    }

    private void Start()
    {
        drawArea = new Rect(0, 0, Screen.width, Screen.height);
        pointerObject = Instantiate(pointerPrefab,new Vector3(transform.position.x,transform.position.y,transform.position.z-10f), Quaternion.identity);



        ////Load user custom gestures
        //string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        //foreach (string filePath in filePaths)
        //    trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
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
                BroAudio.Play(drawSound);
                pointerObject.SetActive(true);
                moveHand.SetActive(false);
                drawingHand.SetActive(true);

                ++strokeId;
                Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation);
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
                //Selection.activeGameObject = tmpGesture.gameObject;

                gestureLinesRenderer.Add(currentGestureLineRenderer);
                isOnPanel = true;
                vertexCount = 0;
            }
            if(Input.GetMouseButtonUp(0))
            {
                BroAudio.Stop(drawSound);
                pointerObject.SetActive(false);
                drawingHand.SetActive(false);
                isOnPanel = false;
                moveHand.SetActive(true);
            }
            if (Input.GetMouseButton(0))
            {
                points.Add(new Point(Input.mousePosition.x, Input.mousePosition.y, strokeId));

                currentGestureLineRenderer.positionCount = ++vertexCount;
                currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
                if (gestureLinesRenderer[0].positionCount > 1)
                    pointerObject.transform.position = gestureLinesRenderer[0].bounds.center;
            }
        }
    }

    public void TryRecognize(string[] gesturesName)
    {
        if (points.Count == 0)
        {
            Debug.Log("안 그리는 중");
            return;
        }

        if (recognized)
            ClearLine();

        recognized = true;
        trainingSet.Clear();
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
        {
            for(int i=0; i<gesturesName.Length;i++)
            {
                if (gestureXml.text.Contains(gesturesName[i]))
                {
                    Debug.Log(gestureXml.text);
                    trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
                }
            }
        }

        Gesture candidate = new Gesture(points.ToArray());

        Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

        if (gestureResult.Score < .8f)
        {
            InitFloatText(gestureLinesRenderer[0].bounds.center, gestureResult.Score,"Error");
            ClearLine();
            return;
        }

        for(int i=0;i<gesturesName.Length;i++)
        {
            if (gestureResult.GestureClass == gesturesName[i])
            {
                GestureActive(gesturesName[i],gestureResult.Score*100);
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

    private void GestureActive(string gestureName,float percent)
    {
        if (gestureName == "VerticalLine")
        {
            RaycastHit hit = new RaycastHit();
            Vector3 direction = (gestureLinesRenderer[0].bounds.center - Camera.main.transform.position).normalized;

            Debug.Log("VerticalLine");
            if (Physics.SphereCast(Camera.main.transform.position, 3f, direction, out hit, Mathf.Infinity, whatIsDrawArea))
            {
                if (hit.collider.CompareTag("DrawArea") && hit.collider.TryGetComponent<ElectricArea>(out ElectricArea area))
                {
                    if (area.CanLightOn(Line.Vertical))
                    {
                        BroAudio.Play(bridgeInitSound);
                        InitFloatText(gestureLinesRenderer[0].bounds.center, percent);
                        pointerObject.SetActive(false);
                    }
                    InitFloatText(gestureLinesRenderer[0].bounds.center, percent, "LineError");
                }
            }
            else
                InitFloatText(gestureLinesRenderer[0].bounds.center, percent, "BridgeError");
            ClearLine();
        }
        if (gestureName == "Apple")
        {
            Debug.Log("시바");
            GameObject b = Instantiate(applePrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(b.transform.position, percent);
            ClearLine();
        }
        if (gestureName == "Flower")
        {
            Debug.Log("시바");
            GameObject b = Instantiate(flowerObject, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(b.transform.position, percent);

            ClearLine();
        }
        if (gestureName == "Tree")
        {
            Debug.Log("시바");
            GameObject b = Instantiate(treeObject, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(b.transform.position, percent);
            BroAudio.Play(bridgeInitSound);

            ClearLine();
        }
        if (gestureName == "Animal")
        {
            
        }
        if (gestureName == "MetalPlate")
        {
            Debug.Log("시바");
            if (metalObj != null) Destroy(metalObj);
            metalObj = Instantiate(ironPlatePrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            metalObj.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(metalObj.transform.position, percent);
            BroAudio.Play(bridgeInitSound);

            ClearLine();
        }
        if (gestureName == "Carrot")
        {
            Debug.Log("시바");
            GameObject b = Instantiate(carrotPrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(b.transform.position, percent);

            ClearLine();
        }
        
        if (gestureName == "Pepper")
        {
            Debug.Log("시바");
            GameObject b = Instantiate(pepperPrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.transform.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(b.transform.position, percent);

            ClearLine();
        }
        if (gestureName == "Bridge")
        {
            RaycastHit hit = new RaycastHit();
            Debug.Log("들어옴");
            Vector3 direction = (gestureLinesRenderer[0].bounds.center - Camera.main.transform.position).normalized;

            if (Physics.SphereCast(Camera.main.transform.position, 3f, direction, out hit, Mathf.Infinity,whatIsDrawArea))
            {
                Debug.Log("감지됨");
                if (hit.collider.CompareTag("DrawArea"))
                {
                    if (currentBridgeObject != null) currentBridgeObject.SetActive(false);
                    currentBridgeObject = hit.collider.GetComponent<BridgeAera>().bridge;
                    currentBridgeObject.SetActive(true);
                    BroAudio.Play(bridgeInitSound);
                    InitFloatText(currentBridgeObject.transform.position, percent);
                    pointerObject.SetActive(false);
                }
            }
            else
                InitFloatText(gestureLinesRenderer[0].bounds.center, percent,"BridgeError");
            ClearLine();
        }
        if (gestureName == "HorizontalLine")
        {
            RaycastHit hit = new RaycastHit();
            Debug.Log("들어옴");
            Vector3 direction = (gestureLinesRenderer[0].bounds.center - Camera.main.transform.position).normalized;

            if (Physics.SphereCast(Camera.main.transform.position, 3f, direction, out hit, Mathf.Infinity, whatIsDrawArea))
            {
                Debug.Log("감지됨");
                if (hit.collider.CompareTag("DrawArea") && hit.collider.TryGetComponent<ElectricArea>(out ElectricArea area))
                {
                    if(area.CanLightOn(Line.Horizontal))
                    {
                        BroAudio.Play(bridgeInitSound);
                        InitFloatText(gestureLinesRenderer[0].bounds.center, percent);
                        pointerObject.SetActive(false);
                    }
                    InitFloatText(gestureLinesRenderer[0].bounds.center, percent, "LineError");
                }
            }
            else
                InitFloatText(gestureLinesRenderer[0].bounds.center, percent, "BridgeError");
            ClearLine();
        }
        
        if (gestureName == "ChickSummon")
        {
            Transform b = Instantiate(chickPrefab, gestureLinesRenderer[0].bounds.center, Quaternion.identity);
            b.DOScale(0, .2f).From().SetEase(Ease.OutBack);
            InitFloatText(b.transform.position, percent);

            ClearLine();
        }

        if (gestureName == "BalloonObj")
        {
            RaycastHit hit = new RaycastHit();

            Vector3 direction = (gestureLinesRenderer[0].bounds.center - Camera.main.transform.position).normalized;

            if (Physics.SphereCast(Camera.main.transform.position, 10f, direction, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("슝");
                Transform b = Instantiate(balloonPrefab, hit.collider.transform);
                if(hit.collider.TryGetComponent(out StaticAnimalObjects staticAnimal))
                    staticAnimal.OnflYAnimal?.Invoke();
                b.DOScale(0, .2f).From().SetEase(Ease.OutBack);
                InitFloatText(b.transform.position, percent);
            }
            else
                InitFloatText(gestureLinesRenderer[0].bounds.center, percent, "Balloon");
            ClearLine();
        }
        ClearLine();
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

    public void InitFloatText(Vector3 spawnPos,float percent,string name="Percent")
    {
        if(name == "Percent" && percent > 95f)
            Instantiate(floatText, new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 1f), Quaternion.identity).GetComponent<TextMeshPro>().text = $"<color=green>{percent.ToString().Substring(0, 2)}% 일치</color>";
        else if (name == "Percent")
            Instantiate(floatText,new Vector3(spawnPos.x,spawnPos.y,spawnPos.z - 1f),Quaternion.identity).GetComponent<TextMeshPro>().text = $"{percent.ToString().Substring(0,2)}% 일치";
        else if(name == "BridgeError")
            Instantiate(floatText, new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 1f), Quaternion.identity).GetComponent<TextMeshPro>().text = $"<color=red>생성 가능 지역이 아닙니다.</color>";
        else if (name == "LineError")
            Instantiate(floatText, new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 1f), Quaternion.identity).GetComponent<TextMeshPro>().text = $"<color=red>모양이 맞지 않습니다.</color>";
        else if (name == "Balloon")
            Instantiate(floatText, new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 1f), Quaternion.identity).GetComponent<TextMeshPro>().text = $"<color=red>물체가 닿지 않았습니다</color>";
        else if(name=="Error")
            Instantiate(floatText, new Vector3(spawnPos.x, spawnPos.y, spawnPos.z - 1f), Quaternion.identity).GetComponent<TextMeshPro>().text = $"<shake>80% 이상 일치해야 합니다 ({(percent*100).ToString().Substring(0,2)}%)</shake>";
    }
    private void OnDrawGizmos()
    {
        if (gestureLinesRenderer.Count < 1) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, gestureLinesRenderer[0].bounds.center - Camera.main.transform.position);
        Gizmos.DrawWireSphere(Camera.main.transform.position, 10f);
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
