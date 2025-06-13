using UnityEngine;

public class SlidingPlatform : MonoBehaviour
{
    public Transform platform; // �ü� ���� Transform
    public float maxAngle = 15f; // �ִ� ���� ����
    public float rotateSpeed = 5f; // ȸ�� �ӵ�
    public Vector3 boxSize = new Vector3(2.5f, 1f, 1f); // ���� ���� ũ��
    public LayerMask detectionLayer; // ������ ���̾�(Player ����)

    private void Update()
    {
        Vector3 center = platform.position + Vector3.up * 0.5f;

        Collider[] hits = Physics.OverlapBox(center, boxSize / 2f, Quaternion.identity, detectionLayer);

        float leftWeight = 0f;
        float rightWeight = 0f;

        foreach (var hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb == null) continue;

            Vector3 localPos = platform.InverseTransformPoint(hit.transform.position);

            if (localPos.x < -0.2f)
                leftWeight += rb.mass;
            else if (localPos.x > 0.2f)
                rightWeight += rb.mass;
        }

        float weightDiff = rightWeight - leftWeight;
        float targetAngle = Mathf.Clamp(weightDiff * 5f, -maxAngle, maxAngle);
        float currentZ = platform.localEulerAngles.z;

        // Convert to signed angle
        if (currentZ > 180f) currentZ -= 360f;

        float newZ = Mathf.Lerp(currentZ, targetAngle, Time.deltaTime * rotateSpeed);
        platform.localRotation = Quaternion.Euler(0, 0, newZ);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = platform.position + Vector3.up * 0.5f;
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Gizmos.DrawCube(center, boxSize);
    }
}
