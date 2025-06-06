using System.Collections;
using UnityEngine;

public class Balloon : ExtendedMono
{
    private LineRenderer _lineRenderer;
    private HingeJoint _hingeJoint;

    private ObjectUnit parentObj;
    private Rigidbody parentObjRigid;

    [Header("²ø·Á¿Ã¶ó°¡´Â Á¤µµ")]
    [SerializeField]
    private float rigidIntensity;
    [Header("²ø·Á¿Ã¶ó°¡´Â Á¤µµ")]
    [SerializeField]
    private float retainTime;

    [Header("ÀÌÆåÆ®")]
    [SerializeField]
    private ParticleSystem balloonExplosion;
    [Header("Ç³¼± ºñÁê¾ó")]
    [SerializeField]
    private GameObject visual;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _hingeJoint = GetComponent<HingeJoint>();
    }

    private void OnEnable()
    {
        parentObj = GetComponentInParent<ObjectUnit>();
        _lineRenderer.positionCount = 2;
        parentObjRigid = parentObj.GetComponent<Rigidbody>();
        _hingeJoint.connectedBody = parentObjRigid;

        parentObjRigid.useGravity = false;
        parentObjRigid.linearVelocity = Vector3.up * rigidIntensity;
        StartSafeCoroutine("EndRoutine", EndRoutine());
    }
    private IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(retainTime);
        balloonExplosion.Play();
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
    private void Update()
    {
        _lineRenderer.SetPosition(0, new Vector3(parentObj.transform.position.x,parentObj.transform.position.y +0.2f, parentObj.transform.position.z));
        _lineRenderer.SetPosition(1, visual.transform.position);
    }

    private void OnDisable()
    {
        parentObj = null;

        _hingeJoint.connectedBody = null;

        parentObjRigid.useGravity = true;
        _lineRenderer.positionCount = 0;
        parentObjRigid.linearVelocity = Vector3.zero;
        parentObjRigid = null;
    }
}
